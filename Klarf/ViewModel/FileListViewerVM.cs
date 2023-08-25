﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Linq;
using Klarf.Model;
using Prism.Events;

namespace Klarf.ViewModel
{
    class FileListViewerVM : ViewModelBase
    {

        #region 필드
        private ObservableCollection<KlarfFile> klarfPaths;
        private KlarfFile selectedFile;
        private string selectedFolderPath;
        private RelayCommand _openFolderCommand;
        private IEventAggregator _eventAggregator;


        #endregion

        #region 속성



        public KlarfFile SelectedFile
        {
            get { return selectedFile; }
            set
            {
                if (selectedFile != value)
                {
                    selectedFile = value;
                    OnPropertyChanged(nameof(selectedFile));

                    _eventAggregator.GetEvent<FileSelectedEvent>().Publish(selectedFile.Path);
                }
            }
        }
        public string SelectedFolderPath
        {
            get
            {
                return selectedFolderPath;
            }
            set
            {
                selectedFolderPath = value;
                OnPropertyChanged(nameof(selectedFolderPath));
            }
        }

     
        public ObservableCollection<KlarfFile> KlarfPaths
        {
            get
            {
                return klarfPaths;
            }
            set
            {
                klarfPaths = value;
                OnPropertyChanged(nameof(KlarfPaths));
            }
        }
        public ICommand OpenFolderCommand
        {
            get
            {
                return _openFolderCommand ?? (_openFolderCommand = new RelayCommand(OpenFolderDialog));
            }
            set
            {

            }
        }
        #endregion
        #region 생성자



        public FileListViewerVM(IEventAggregator eventAggregator)
        {
            
            _eventAggregator = eventAggregator;
             klarfPaths = new ObservableCollection<KlarfFile>();
            OpenFolderCommand = new RelayCommand(OpenFolderDialog);
        }
        public FileListViewerVM()
        {
            
            klarfPaths = new ObservableCollection<KlarfFile>();
            OpenFolderCommand = new RelayCommand(OpenFolderDialog);
        }
        #endregion

        #region 메서드
        
        private void OpenFolderDialog()
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SelectedFolderPath = folderDialog.SelectedPath;
                    LoadFileList(SelectedFolderPath);
                }
            }
        }

        private void LoadFileList(string folderPath)
        {
            string[] klarfExtensions = { ".001", ".tif" };

            if (Directory.Exists(folderPath))
            {
                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (klarfExtensions.Contains(extension))
                    {
                        DateTime updateTime = File.GetLastWriteTime(filePath);
                        klarfPaths.Add(new KlarfFile { Name = Path.GetFileName(filePath), Path = filePath, UpdateTime = updateTime });
                    }
                }
            }
        }




    }
    #endregion
    #region 중첩된 클래스

    #endregion









}
