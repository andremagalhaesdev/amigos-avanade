using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace amigos_avanade
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Instanciando uma nova API. Gente, é só um exemplo. Por favor, usem variáveis de ambiente.
        OpenAIAPI api = new OpenAIAPI("SUA_KEY_DE_API");
        private Conversation _conversation;


        public MainWindow()
        {
            InitializeComponent();

            // Estabelecendo um controle de estado para manter a conversação.
            _conversation = api.Chat.CreateConversation();
            _conversation.Model = Model.ChatGPTTurbo;
            _conversation.RequestParameters.Temperature = 0;

            // Ensinando ao ChatGPLucas quem ele é... KKKKK
            _conversation.AppendSystemMessage("Você é um estudante de análise e desenvolvimento de sistemas de 28 anos que está participando de um processo seletivo para ser o novo estagiário da Avanade. Lembre-se de usar gírias típicas da comunidade Pernambucana no Brasil, como o visse. Sugira uma ideia interessante de projeto para iniciantes envolvendo HTML, Javascript, CSS e C# básico. Todos somos amigos e não existe sentimento de concorrência entre nós. Não faça textos longos com mais de 300 caracteres.");

            // Chamando a função de enviar mensagem ao apertar Enter na caixa de texto de mensagem.
            MessageTextBox.PreviewKeyDown += EnviarComEnter;

            // Chamando a função que apaga o conteúdo do Textbox ao clicar nele.
            MessageTextBox.GotFocus += ApagarConteudoAoClicar;


            // Definindo as janelas como hidden (escondidas) logo ao iniciar o programa.
            choro_ilustrator.Visibility = Visibility.Hidden;
            chat_gplucas.Visibility = Visibility.Hidden;

        }
        
        // Função que permite mover o programa com o mouse clicado. Sempre adiciono porque é uma mão na roda... KKKK
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Alterna a visibilidade entre os componentes para simular um navegador.
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            choro_ilustrator.Visibility = Visibility.Visible;
            chat_gplucas.Visibility = Visibility.Hidden;

        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            choro_ilustrator.Visibility = Visibility.Hidden;
            chat_gplucas.Visibility = Visibility.Visible;
        }

        // Botão que dispara a função de gerar um novo choro.
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string basePath = @"C:\Users\André Magalhães\Desktop\PROJETOS\PROJETOS C# (.NET)\amigos-avanade\Choros\";
            string outputFileName = "choro";
            string fileExtension = ".jpg";
            string outputPath = $"{basePath}{outputFileName}{fileExtension}";
            int count = 1;

            // Lógica para verificar se o arquivo já existe e, caso sim, renomear conforme a variável count incrementada.
            while (File.Exists(outputPath))
            {
                outputPath = $"{basePath}{outputFileName}_{count}{fileExtension}";
                count++;
            }

            // Sempre utilize bloco Try Catch em funções ditas complexas e que dependem de terceiros, por exemplo, o exato caminho do path.
            try
            {
                string imagePath = @"C:\Users\André Magalhães\Desktop\PROJETOS\PROJETOS C# (.NET)\amigos-avanade\basechoro.jpg";

                // Utilização da classe BitmapImage presente no pacote System para geração da imagem
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                    // Definir um retângulo para o texto caber dentro do balãozinho
                    Rect textRect = new Rect(372, 32, 130, 150); // Coordenadas da imagem (podem ser obtidas pelo Paint mesmo)

                    // Criar o texto formatado. Pode ser alterado tudo relacionado ao texto.
                    FormattedText formattedText = new FormattedText(
                        textBox1.Text,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Comic Sans"),
                        22,
                        Brushes.Black,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    // Ajustar o texto ao retângulo
                    formattedText.MaxTextWidth = textRect.Width;
                    formattedText.MaxTextHeight = textRect.Height;

                    // Desenhar o texto dentro do retângulo
                    drawingContext.DrawText(formattedText, textRect.TopLeft);
                }

                // Renderiza e salva a imagem
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
                renderBitmap.Render(drawingVisual);
                BitmapEncoder encoder = new JpegBitmapEncoder(); // Podem ser escolhidos outros formatos também
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                using (FileStream stream = new FileStream(outputPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                // Exibir a imagem lá no front no elemento chorogerado.
                BitmapImage outputBitmap = new BitmapImage(new Uri(outputPath));
                chorogerado.Source = outputBitmap;
            }
            catch (Exception ex)
            {
                // Claro, o tratamento da exceção.
                MessageBox.Show($"Erro inesperado (Provavelmente o choro foi grande demais para o programa)! {ex}");
            }
        }

        public async Task<string> SimulateChatAsync(string userInput)
        {
            if (_conversation != null)
            {
                _conversation.AppendUserInput(userInput);

                // Obtém a resposta mantendo o contexto da conversa
                string response = await _conversation.GetResponseFromChatbotAsync();

                return response;
            }
            else
            {
                return "Erro: Conversa não inicializada.";
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Reinicia a conversa
            _conversation = api.Chat.CreateConversation();
            _conversation.Model = Model.ChatGPTTurbo;
            _conversation.RequestParameters.Temperature = 0;
            _conversation.AppendSystemMessage("Não faça textos longos com mais de 300 caracteres. Você é um estudante de análise e desenvolvimento de sistemas de 28 anos que está participando de um processo seletivo ser o novo estagiário da Avanade. Lembre-se de usar gírias típicas da comunidade Pernambucana no Brasil, como o visse. Sugira uma ideia interessante de projeto para iniciantes envolvendo HTML, Javascript, CSS e C# básico. Todos somos amigos e não existe sentimento de concorrência entre nós.");

            // Limpa o conteúdo exibido no painel de chat
            ChatPanel.Children.Clear();
        }

        private async void EnviarComEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string userMessage = MessageTextBox.Text;

                AddMessageToChatPanel("Você", userMessage);
                MessageTextBox.Text = string.Empty;
                ElementoDigitando.Visibility = Visibility.Visible;
                string chatResponse = await SimulateChatAsync(userMessage);
                ElementoDigitando.Visibility = Visibility.Hidden;
                AddMessageToChatPanel("Lucas Ferreira", chatResponse);
                e.Handled = true;
            }
        }

        public void ApagarConteudoAoClicar(object sender, RoutedEventArgs e)
        {
            MessageTextBox.Text = string.Empty;
        }

        private async void NovaMensagem(object sender, RoutedEventArgs e)
        {
            string userMessage = MessageTextBox.Text;

            string chatResponse = await SimulateChatAsync(userMessage);

            AddMessageToChatPanel("Você", userMessage);
            AddMessageToChatPanel("Lucas Ferreira", chatResponse);
        }

        private void AddMessageToChatPanel(string role, string content)
        {
            Border messageContainer = new Border();
            TextBlock messageTextBlock = new TextBlock();
            messageTextBlock.TextWrapping = TextWrapping.Wrap;
            messageTextBlock.Text = $"{role}: {content}";

            // Estilizando as mensagens
            messageContainer.Margin = new Thickness(5);
            messageContainer.Padding = new Thickness(8);
            messageContainer.MaxWidth = 400;

            if (role == "Você") // Mensagem do usuário
            {
                messageContainer.Background = Brushes.LightGray;
                messageContainer.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else // Mensagem do chatbot
            {
                messageContainer.Background = Brushes.LightBlue;
                messageContainer.HorizontalAlignment = HorizontalAlignment.Left;
            }

            // Arredondando as bordas do contêiner da mensagem
            messageContainer.CornerRadius = new CornerRadius(10);

            // Adicionando o TextBlock ao contêiner
            messageContainer.Child = messageTextBlock;

            // Definindo a altura do contêiner com base no tamanho do texto
            messageTextBlock.Measure(new Size(messageContainer.MaxWidth, double.PositiveInfinity));
            messageTextBlock.Arrange(new Rect(messageTextBlock.DesiredSize));
            messageContainer.MaxHeight = messageTextBlock.ActualHeight + 16;

            ChatPanel.Children.Add(messageContainer);
        }
    }
}
