using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ModList
{
    public class UiLoader
    {
        private const string _winBundle = "ModList.Resources.modlist-win.bundle";
        private const string _osxBundle = "ModList.Resources.modlist-osx.bundle";
        private const string _linuxBundle = "ModList.Resources.modlist-linux.bundle";
        
        private static readonly Lazy<UiLoader> s_lazy = new(() => new UiLoader());

        private AssetBundle? _assetBundle;
        
        private UiLoader()
        {
        }

        public static UiLoader Instance
        {
            get => s_lazy.Value;
        }

        public void LoadUiAssetBundle()
        {
            PlatformID platform = Environment.OSVersion.Platform;

            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            string bundleName = platform switch
            {
                PlatformID.Win32NT => _winBundle,
                PlatformID.MacOSX => _osxBundle,
                PlatformID.Unix => _linuxBundle,
                _ => throw new PlatformNotSupportedException("Unsupported platform, cannot load asset bundles.")
            };

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(bundleName)!;
            _assetBundle = AssetBundle.LoadFromStream(stream);
        }

        public GameObject LoadUiAsset()
        {
            if (_assetBundle == null || !_assetBundle)
                LoadUiAssetBundle();

            return _assetBundle!.LoadAsset<GameObject>("ModList");
        }
    }
}
