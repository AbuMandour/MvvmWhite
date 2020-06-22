using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using WhiteMvvm.Bases;
using WhiteMvvm.Configuration;
using WhiteMvvm.Exceptions;
using WhiteMvvm.Services.Dialog;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Navigation;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Resolve
{
    public class ReflectionResolve : IReflectionResolve
    {
        public Page CreatePage(Type viewModelType)
        {
            var viewFolderName = ConfigurationManager.Current.ViewsFolderName;
            var viewModelFolderName = ConfigurationManager.Current.ViewModelFolderName;
            var viewFileName = ConfigurationManager.Current.ViewsFileName;
            var viewModelFileName = ConfigurationManager.Current.ViewModelFileName;
            var pageType = GetTypeFromAssembly(viewModelType, viewModelFolderName, viewFolderName, viewModelFileName, viewFileName);
            var page = GetPageInstance(pageType);
            return page;
        }
        public BaseViewModel CreateViewModelFromPage(Type pageType)
        {
            try
            {
                var viewFolderName = ConfigurationManager.Current.ViewsFolderName;
                var viewModelFolderName = ConfigurationManager.Current.ViewModelFolderName;
                var viewFileName = ConfigurationManager.Current.ViewsFileName;
                var viewModelFileName = ConfigurationManager.Current.ViewModelFileName;

                var viewmodelType = GetTypeFromAssembly(pageType, viewFolderName, viewModelFolderName, viewFileName, viewModelFileName);
                var viewModel = GetViewModelInstance(viewmodelType);
                return viewModel;
            }
            catch (ReflectionResolveException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new ReflectionResolveException($"View Model for {pageType.Name} not found", exception);
            }
        }
        private static Type GetTypeFromAssembly(Type typeFrom, string folderNameToBeReplaced,
            string replacedFolderName, string fileNameToBeReplaced, string replacedFileName)
        {
            if (typeFrom == null || string.IsNullOrEmpty(typeFrom.Namespace) || string.IsNullOrEmpty(typeFrom.Name))
            {
                throw new ReflectionResolveException($"Cannot locate page type", new NullReferenceException($"{typeFrom} or {typeFrom} name can not be null"));
            }
            var fileName = typeFrom.Name.Replace(fileNameToBeReplaced, replacedFileName);
            var namespaceName = typeFrom.Namespace.Replace(folderNameToBeReplaced, replacedFolderName);

            var fileFullName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, fileName);
            var assemblyName = typeFrom.Assembly.FullName;
            var fileAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", fileFullName, assemblyName);

            var newType = Type.GetType(fileAssemblyName);
            return newType;
        }
        private static Page GetPageInstance(Type pageType)
        {
            if (pageType == null)
            {
                throw new ReflectionResolveException($"Cannot locate page type", new NullReferenceException($"page type can not be null"));
            }
            var page = Activator.CreateInstance(pageType);
            return page as Page;
        }
        private static BaseViewModel GetViewModelInstance(Type viewModelType)
        {
            if (viewModelType == null)
            {
                throw new ReflectionResolveException($"Cannot locate view model type", new NullReferenceException("view model type can not be null"));
            }
            var viewModel = LocatorService.Instance.Resolve(viewModelType) as BaseViewModel;
            return viewModel;
        }
    }
}
