﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using FileHelpers;

using storage_app.Models;
using storage_app.Services;
using storage_app.Utils;
using storage_app.Utils.Objects;
using storage_app.ViewModels.Actions;

namespace storage_app.ViewModels
{
    internal class StorageDataGridViewModel : ViewModelBase
    {
        private SelectedProductActionViewModel _selectedProductActionViewModel;
        public SelectedProductActionViewModel SelectedProductActionViewModel
        {
            get { return _selectedProductActionViewModel; }
            set
            {
                _selectedProductActionViewModel = value;
                OnPropertyChanged(nameof(SelectedProductActionViewModel));
            }
        }

        private List<Product> _products = new();
        public List<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product _selectedProduct = new();
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private readonly IProductService productService;

        private StartExporting? _startExportingCommand;
        public StartExporting StartExportingCommand
        {
            get
            {
                if (_startExportingCommand == null)
                    _startExportingCommand = new StartExporting(_ => { });
                return _startExportingCommand;
            }
            set
            {
                _startExportingCommand = value;
            }
        }
        public StorageDataGridViewModel(
            IProductService productService,
            SelectedProductActionViewModel selectedProductActionViewModel)
        {
            _startExportingCommand = new((_) => StartExporting());
            _selectedProductActionViewModel = selectedProductActionViewModel;
            this.productService = productService;
            GetProducts();
        }

        public void StartExporting()
        {

            var path = ChoosePath();
            if (path != "")
            {
                var engine = new FileHelperEngine<Product>();
                engine.HeaderText = engine.GetFileHeader();
                engine.WriteFile(path, _products);
                ShowMessage.DefaultMessage($"File saved on {path}");
            }
            else
            {
                ShowMessage.ErrorMessage("No path selected!");
            }
            
        }

        public static string ChoosePath()
        {
            var initial = "";
            string path = "";
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "report.csv",
                Title = "Select a Directory",
                InitialDirectory = initial
            };
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                if (!path.EndsWith(".csv"))
                {
                    path += ".csv";
                }
            }
            return path;
        }

        public void GetProducts(Filter? filter = null)
        {
            if (filter == null)
            {
                var task = Task.Run(async () => await productService.GetProducts());
                Products = task.Result;
            }
            else
            {
                var task = Task.Run(
                    async () =>
                    await productService
                    .GetProductsFiltered(
                        id: filter.Id,
                        description: filter.Description,
                        category: filter.Category.Description
                        )
                    );
                Products = task.Result;
            }

            if (Products.Count > 0) SelectedProduct = Products[0];
            else SelectedProduct = new Product();
            
            SelectedProductActionViewModel.SelectedProductCommand.Execute(SelectedProduct);
        }
        
    }
    internal class StartExporting : CommandExecutor
    {
        public StartExporting(Action<object?> action) : base(action) { }
    }
}
