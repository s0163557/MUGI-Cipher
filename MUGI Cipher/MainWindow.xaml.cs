using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MUGI_Cipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            MUGIAlgorithm mugi = new MUGIAlgorithm();
            mugi.K1 = Convert.ToUInt64(EncryptKey.Text);
            mugi.I = Convert.ToUInt64(EncryptVector.Text);
            ulong[] temp = mugi.Encrypt(EncryptText.Text);
            StringBuilder sb = new StringBuilder();
            foreach (ulong value in temp)
                sb.Append(value.ToString() + "-");
            sb.Remove(sb.Length - 1, 1);
            EncryptResult.Text = sb.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MUGIAlgorithm mugi = new MUGIAlgorithm();
            mugi.K1 = Convert.ToUInt64(DecryptKey.Text);
            mugi.I = Convert.ToUInt64(DecryptVector.Text);
            string[] words = DecryptText.Text.Split("-");
            ulong[] blocks = new ulong[words.Length];
            for (int i = 0; i < words.Length; i++)
                blocks[i] = Convert.ToUInt64(words[i]);
            DecryptResult.Text = mugi.Decrypt(blocks);
        }
    }
}