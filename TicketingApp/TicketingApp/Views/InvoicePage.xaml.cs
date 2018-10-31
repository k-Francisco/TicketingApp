using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketingApp.ViewModels;
using Xamarin.Forms;

namespace TicketingApp.Views
{
    public partial class InvoicePage : ContentPage
    {
        public InvoicePage()
        {
            InitializeComponent();

            var vm = BindingContext as InvoicePageViewModel;

            vm.SignatureFromStream = async () =>
            {
                if(signaturePad.Points.Count() > 0)
                {
                    using (var stream = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png))
                    {
                        return await ReadFully(stream);
                    }
                }

                return await Task.Run(() => (byte[])null);
            };
        }

        public static async Task<byte[]> ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
