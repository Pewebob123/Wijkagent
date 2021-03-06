﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WijkAgent
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        #region ShowLoadingScreen
        public void ShowLoadingScreen()
        {
            try
            {
                this.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region HideLoadingScreen
        public void HideLoadingScreen()
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion
    }
}
