﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Http;
using System.Threading;
using Xr.Common;

namespace Xr.RtManager
{
    public partial class MenuEdit : Form
    {
        public MenuEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;
        public MenuEntity menu { get; set; }

        private void MenuEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);
            dcMenu.DataType = typeof(MenuEntity);
            String extId = "";
            if(menu!=null && menu.id !=null)
                extId = menu.id;
            //获取下拉框数据
            String param = "?extId=" + extId;
            String url = AppContext.AppConfig.serverUrl + "sys/sysMenu/treeData" + param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    treeParent.Properties.DataSource = objT["result"].ToObject<List<MenuEntity>>();
                    treeParent.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    treeParent.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    treeParent.Properties.DisplayMember = "name";  //要在树里展示的
                    treeParent.Properties.ValueMember = "id";    //对应的value
                    treeParent.EditValue = "1";

                    if (menu != null)
                    {
                        if (menu.id != null)
                        {
                            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                data = HttpClass.httpPost(AppContext.AppConfig.serverUrl + "sys/sysMenu/getMenu?menuId=" + menu.id);
                                return data;

                            }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                            {
                                objT = JObject.Parse(data2.ToString());
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    menu = objT["result"].ToObject<MenuEntity>();
                                    dcMenu.SetValue(menu);
                                    cmd.HideOpaqueLayer();
                                }
                                else
                                {
                                    cmd.HideOpaqueLayer();
                                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                                }
                            });
                        }
                        else
                        {
                            menu.isShow = "1";
                            dcMenu.SetValue(menu);
                            cmd.HideOpaqueLayer();
                        }
                    }
                    else
                    {
                        menu = new MenuEntity();
                        cmd.HideOpaqueLayer();
                    }
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!dcMenu.Validate())
            {
                return;
            }
            dcMenu.GetValue(menu);
            var index = treeParent.Properties.GetIndexByKeyValue(menu.parentId);
            List<MenuEntity> entityList = treeParent.Properties.TreeList.DataSource as List<MenuEntity>;
            menu.parentIds = entityList[index].parentIds + menu.parentId + ",";
            String param = PackReflectionEntity<MenuEntity>.GetEntityToRequestParameters(menu, true);
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(AppContext.AppConfig.serverUrl + "sys/sysMenu/save?" + param);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                cmd.HideOpaqueLayer();
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            //Form loadingForm = null;
            //System.Windows.Forms.Control loadingPan = null;
            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1) return;

            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    cmd.HideOpaqueLayer();
                    if (ex.InnerException != null)
                        throw new Exception(ex.InnerException.Message);
                    else
                        throw new Exception(ex.Message);
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }
    }
}
