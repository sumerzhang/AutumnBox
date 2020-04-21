﻿using AutumnBox.Basic.ManagedAdb.CommandDriven;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AutumnBox.Basic.ManagedAdb
{
    /// <summary>
    /// Adb管理器
    /// </summary>
    public interface IAdbManager : IDisposable
    {
        /// <summary>
        /// 获取管理器
        /// </summary>
        ICommandProcedureManager OpenCommandProcedureManager();

        /// <summary>
        /// ADB客户端文件夹
        /// </summary>
        DirectoryInfo AdbClientDirectory { get; }

        /// <summary>
        /// 获取ADB服务地址
        /// </summary>
        IPEndPoint ServerEndPoint { get; }
    }
}
