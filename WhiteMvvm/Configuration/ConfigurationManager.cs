using System;
using Acr.UserDialogs;

namespace WhiteMvvm.Configuration
{
    public sealed class ConfigurationManager
    {
        private static readonly Lazy<ConfigurationManager> Lazy = new Lazy<ConfigurationManager>(() => new ConfigurationManager());
        private string _viewsFolderName;
        private string _viewModelFolderName;
        private string _viewsFileName;
        private string _loadingDisplay;
        private string _viewModelFileName;


        public static ConfigurationManager Current => Lazy.Value;
        private ConfigurationManager()
        {
        }
        public bool UseBasicIndicator { get; set; } = false;
        public MaskType IndicatorMaskType { get; set; } = MaskType.Gradient;
        public string ViewsFolderName
        {
            get => string.IsNullOrEmpty(_viewsFolderName) ? "Views" : _viewsFolderName;
            set => _viewsFolderName = value;
        }
        public string ViewsFileName
        {
            get => string.IsNullOrEmpty(_viewsFileName) ? "Page" : _viewsFileName;
            set => _viewsFileName = value;
        }
        public string ViewModelFolderName
        {
            get => string.IsNullOrEmpty(_viewModelFolderName) ? "ViewModels" : _viewModelFolderName;
            set => _viewModelFolderName = value;
        }
        public string ViewModelFileName
        {
            get => string.IsNullOrEmpty(_viewModelFileName) ? "ViewModel" : _viewModelFileName;
            set => _viewModelFileName = value;
        }
        public string LoadingDisplay
        {
            get => string.IsNullOrEmpty(_loadingDisplay) ? "Loading..." : _loadingDisplay;
            set => _loadingDisplay = value;
        }
    }
}
