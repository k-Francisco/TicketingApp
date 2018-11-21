using System;
using TicketingApp.ViewModels;
using Xamarin.Forms;

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
        }
    }
}
