﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Huoyaoyuan.AdmiralRoom;
using Huoyaoyuan.AdmiralRoom.Officer;
using Meowtrix.ComponentModel;

namespace RawApiViewer
{
    class RawApiViewModel : NotificationObject
    {
        public static RawApiViewModel Instance { get; set; }
        private RawApiViewModel()
        {
            Staff.API("").Subscribe(AddSession);
        }
        private void AddSession(CachedSession session)
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                _list.Insert(0, new ApiModel(session));
                int max = SaveCount;
                if (SelectedIndex >= max) max = SelectedIndex + 1;
                while (_list.Count > max)
                    _list.RemoveAt(max);
            });
        }

        #region SaveCount
        private int _savecount = 20;
        public int SaveCount
        {
            get { return _savecount; }
            set
            {
                if (value <= 1) value = 1;
                if (_savecount != value)
                {
                    _savecount = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        private ObservableCollection<ApiModel> _list = new ObservableCollection<ApiModel>();
        public IReadOnlyList<ApiModel> List => _list;

        #region SelectedIndex
        private int _selectedindex = -1;
        public int SelectedIndex
        {
            get { return _selectedindex; }
            set
            {
                if (_selectedindex != value)
                {
                    _selectedindex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        #endregion

        public ApiModel SelectedItem => List[SelectedIndex];
    }
}
