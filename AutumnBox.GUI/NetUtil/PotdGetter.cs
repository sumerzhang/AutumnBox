﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/1/15 23:35:50 (UTC +8:00)
** desc： ...
*************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutumnBox.GUI.NetUtil
{
    public class PotdGetterResult
    {
        public MemoryStream ImageMemoryStream { get; internal set; }
        public bool CanbeClosed { get; internal set; }
        public string ClickUrl { get; internal set; }
    }
    internal class PotdGetter : RemoteDataGetter<PotdGetterResult>
    {
        [JsonObject(MemberSerialization.OptOut)]
        private class RemoteAdInfo
        {
            [JsonProperty("canbeClosed")]
            public bool CanbeClosed { get; set; }
            [JsonProperty("link")]
            public string Link { get; set; }
            [JsonProperty("click")]
            public string Click { get; set; }
        }
#if TEST_LOCAL_API
        private const string apiUrl = "http://localhost:24010/api/ad/";
#else
        private const string apiUrl = "http://atmb.top/api/ad/";
#endif
        public override PotdGetterResult LocalMethod()
        {
            var html = webClient.DownloadString(apiUrl);
            var remoteInfo = (RemoteAdInfo)JsonConvert.DeserializeObject(html, typeof(RemoteAdInfo));
            var imgData = webClient.DownloadData(remoteInfo.Link);
            return new PotdGetterResult()
            {
                ClickUrl = remoteInfo.Click,
                CanbeClosed = remoteInfo.CanbeClosed,
                ImageMemoryStream = new MemoryStream(imgData)
            };
        }

        public override PotdGetterResult NetMethod()
        {
            var html = webClient.DownloadString(apiUrl);
            var remoteInfo = (RemoteAdInfo)JsonConvert.DeserializeObject(html, typeof(RemoteAdInfo));
            var imgData = webClient.DownloadData(remoteInfo.Link);
            return new PotdGetterResult()
            {
                ClickUrl = remoteInfo.Click,
                CanbeClosed = remoteInfo.CanbeClosed,
                ImageMemoryStream = new MemoryStream(imgData)
            };
        }
    }
}