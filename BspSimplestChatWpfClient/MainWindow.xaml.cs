using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.AspNet.SignalR.Client;

namespace BspSimplestChatWpfClient
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      this.DataContext = new ViewModel();
    }
  }

 internal class ViewModel : ViewModelBase
  {
    private string _text;
    private SynchronizationContext _syncContext;
    private bool _isReady;
    private IHubProxy _hub;

    public ViewModel()
    {
      this._syncContext = SynchronizationContext.Current;

      this.SendCommand = new RelayCommand(
        execute: () => this._hub.Invoke("Send", this._text),
        canExecute: () => this._isReady);
      this.Messages = new ObservableCollection<string>();

      var connection = new HubConnection("http://bspsimplestchat.azurewebsites.net/");
      this._hub = connection.CreateHubProxy("ChatHub");
      this._hub.On<string>("MessageReceived", message => {
        this._syncContext.Post(ignoredParameter => this.Messages.Add(message), null);
      });

      connection.Start().ContinueWith(task => {
        this._isReady = true;
        this._syncContext.Post(ignoredParameter => this.SendCommand.RaiseCanExecuteChanged(), null);
      });
    }


    public RelayCommand SendCommand { get; private set; }


    public IList<string> Messages { get; private set; }


    public string Text
    {
      get { return this._text; }
      set { this.Set(ref this._text, value); }
    }
  }
}
