using System;
using TicketingApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TicketingApp.Views
{
    public partial class TicketsPage : ContentPage
    {
        public TicketsPage()
        {
            InitializeComponent();

            var account = new ToolbarItem()
            {
                Icon = "ic_user.png",
                Command = (BindingContext as TicketsPageViewModel).AccountCommand,
            };

            this.ToolbarItems.Add(account);

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
        }
    }
}
