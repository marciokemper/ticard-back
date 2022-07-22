using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Enum
{
    public enum TipoPessoa
    {
        Fisica = 1,
        Juridica = 2
    }

    public enum TipoClasse
    {
        Fixo = 1,
        Movel = 2,
        Misto = 3
    }

    public enum TipoDocumento
    {
        Pecas = 1,
        Servicos = 2,
        Ambos = 3,
    }



    public enum TipoInformacao
    {
        Data = 1,
        Inteiro = 2,
        Valor = 3
    }

    public enum SituacaoArquivo
    {
        Aguardando = 1,
        ProcessadoComErro = 2,
        ProcessadoComSucesso = 3
    }

    public enum StatusArquivoColetado
    {
        NaoProcessado = 1,
        ProcessadoComSucesso = 2,
        ProcessadoComErro = 3
    }

    public enum TipoProcessamento
    {
        Resumido = 1,
        Detalhado = 2
    }

    public enum TipoFornecedor
    {
        Telefone = 1,
        Gas = 2,
        Energia = 3,
        Agua = 4,
        Ipva = 5,
        Multa = 6,
        Licenciamento = 7
    }
    public enum StorageType
    {
        AWS = 1,
        Local = 2,
        Azure = 3,
        Nexxera = 4
    }
    public enum FieldParserType
    {
        RegexParsing,
        ListParsing,
        TableParsing,
        TextAreaParsing,
        FixedAreaParsing,
        MathParsing,
    }
    public enum InvoiceStatus
    {
        Unknown=0, //Desconhecido
        Pending =1, //A Pagar
        Paid =2, //Pago
        Disputed =3, //Contestado
        Late =4, //Atrasado
        Exempt =5, //Isento
        Automatic = 6 //Débito Automático
    }
    public enum  FetchErrorType
    {
        OpeningChrome,
        OpeningURL,
        LoggingIn,
        BrowsingWebSite,
        DownloadingFile,
        FindingElement,
    }
    public enum RobotsStatus
    {
        Unknown,
        Running,
        NotStarting,
        Crashing,
        DownloadingErrors,
        BrowsingErrors,
        LoggingInError,
        WrongUserData,
        MissingUserData
    }
    public enum RobotLogType
    {
        Information,
        Debug,
        Warning,
        Error,
        Fatal,
        Context,
    }
    public enum FluxoDocumento
    {
        Aguardando=1,
        Aprovado=2,
        Rejeitado = 3
    }
}
