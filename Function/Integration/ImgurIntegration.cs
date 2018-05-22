using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using Function.Util;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;

namespace Function.Integration {

    internal static class ImgurIntegration {

        private static readonly Configuration ConfigManager;
        private static readonly KeyValueConfigurationCollection Config;

        static ImgurIntegration() {
            ConfigManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Config = ConfigManager.AppSettings.Settings;
        }

        public static async Task<IImage> UploadImage(Image image) {
            try {
                var client = new ImgurClient(Config["imgurClientId"].Value, Config["imgurClientSecret"].Value);
                var endpoint = new ImageEndpoint(client);
                return await endpoint?.UploadImageBinaryAsync(image.ToByteArray());
            } catch (ImgurException ie) {
                Debug.WriteLine("An error occured while uploading to ImgurIntegration");
                Debug.WriteLine(ie.Message);
                throw;
            }
        }
    }
}
