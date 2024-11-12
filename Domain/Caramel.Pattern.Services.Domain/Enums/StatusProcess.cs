﻿using System.ComponentModel;

namespace Caramel.Pattern.Services.Domain.Enums
{
    public enum StatusProcess
    {
        [Description("Processado com Sucesso")]
        Success = 0,
        [Description("Request Inválida")]
        InvalidRequest = 1,
        [Description("Erro ao Processar")]
        Failure = 2,
        [Description("Erro no Banco de dados")]
        DbFailure = 3,
        [Description("Não Encontrado")]
        NoContent = 4,
        [Description("Erro ao enviar o Email")]
        SESFailure = 5,
        [Description("Não autorizado")]
        Unauthorized = 6
    }
}
