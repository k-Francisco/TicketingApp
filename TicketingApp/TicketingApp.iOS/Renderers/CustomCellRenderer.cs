using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace TicketingApp.iOS.Renderers
{
    class CustomCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }
    }
}