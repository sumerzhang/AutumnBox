﻿using AutumnBox.Basic;
using AutumnBox.Basic.Devices;
using AutumnBox.Images.DynamicIcons;
using AutumnBox.UI;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutumnBox
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        Core core;
        RateBox rateBox;
        public Window1()
        {
            InitializeComponent();
            core = new Core();
            InitEvents();
            core.dl.Start();
            ChangeButtonByStatus(DeviceStatus.NO_DEVICE);
        }

        private void CustomTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void LabelClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.LabelClose.Background = new ImageBrush
            {
                ImageSource = Tools.BitmapToBitmapImage(DyanamicIcons.close_normal)
            };
        }

        private void LabelClose_MouseEnter(object sender, MouseEventArgs e)
        {
            this.LabelClose.Background = new ImageBrush
            {
                ImageSource = Tools.BitmapToBitmapImage(DyanamicIcons.close_selected)
            };
        }

        private void LabelMin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void LabelMin_MouseEnter(object sender, MouseEventArgs e)
        {
            this.LabelMin.Background = new ImageBrush
            {
                ImageSource = Tools.BitmapToBitmapImage(DyanamicIcons.min_selected)
            };
        }

        private void LabelMin_MouseLeave(object sender, MouseEventArgs e)
        {
            this.LabelMin.Background = new ImageBrush
            {
                ImageSource = Tools.BitmapToBitmapImage(DyanamicIcons.min_normal)
            };
        }

        private void DevicesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.DevicesListBox.SelectedIndex != -1)
            {
                new Thread(SetUIByDevices).Start();
                rateBox = new RateBox();
                rateBox.Owner = this;
                rateBox.ShowDialog();
            }
            else
            {
                this.AndroidVersionLabel.Content = Application.Current.FindResource("PleaseSelectedADevice").ToString();
                this.CodeLabel.Content = Application.Current.FindResource("PleaseSelectedADevice").ToString();
                this.ModelLabel.Content = Application.Current.FindResource("PleaseSelectedADevice").ToString();
                ChangeButtonByStatus(DeviceStatus.NO_DEVICE);
            }
        }

        private void buttonPushFileToSdcard_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Reset();
            fileDialog.Title = "选择一个文件";
            fileDialog.Filter = "刷机包/压缩包文件(*.zip)|*.zip|镜像文件(*.img)|*.img|全部文件(*.*)|*.*";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == true)
            {
                Thread t = new Thread(new ParameterizedThreadStart(core.PushFileToSdcard));
                string[] args = { this.DevicesListBox.SelectedItem.ToString(),fileDialog.FileName};
                t.Start(args);
                this.rateBox = new RateBox();
                this.rateBox.Owner = this;
                this.rateBox.ShowDialog();
            }
            else
            {
                return;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void buttonRebootToRecovery_Click(object sender, RoutedEventArgs e)
        {
            core.Reboot(DevicesListBox.SelectedItem.ToString(),Basic.Other.RebootOptions.Recovery);
        }

        private void buttonRebootToBootloader_Click(object sender, RoutedEventArgs e)
        {
            core.Reboot(DevicesListBox.SelectedItem.ToString(), Basic.Other.RebootOptions.Bootloader);
        }

        private void buttonRebootToSystem_Click(object sender, RoutedEventArgs e)
        {
            core.Reboot(DevicesListBox.SelectedItem.ToString(), Basic.Other.RebootOptions.System);
        }
    }
}
