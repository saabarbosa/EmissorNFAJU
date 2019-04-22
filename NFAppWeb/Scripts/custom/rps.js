var registrosObtidosParaLoteRps = false;
var registrosObtidosParaPrestador = false;
var registrosObtidosParaTomador = false;
var camposFormCadastro = {
    Id: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    LoteRps: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    }
};
var colunasTabelaPrincipal = [
    {
        data: 'Id',
        name: 'Id'
    },
    {
        data: 'IdLoteRps',
        name: 'IdLoteRps'
    },
    {
        data: 'Numero',
        name: 'Numero'
    },
    {
        data: 'Serie',
        name: 'Serie'
    },
    {
        data: 'Tipo',
        name: 'Tipo'
    },
    {
        data: 'DataEmissao',
        name: 'DataEmissao'
    },
    {
        data: 'ValorServico',
        name: 'ValorServico'
    },
    {
        data: 'CpfCnpj_Prestador',
        name: 'CpfCnpj_Prestador'
    },
    {
        data: 'CpfCnpj_Tomador',
        name: 'CpfCnpj_Tomador'
    },

    {
        render: renderColunaOpcoes
    }
];
metodoGet = 'Rps/Get';
metodoInsert = 'Rps/Insert';

function ajustarBotoes(somenteLeitura) {
    if (somenteLeitura) {
        $('.campoCadastro').css('display', 'none');
        $('.campoLeitura').css('display', '');

        $('#btnSalvar').css('display', 'none');
        $('#btnCancelar').removeClass('btn-danger');
        $('#btnCancelar').addClass('btn-primary');
        $('#btnCancelar > label').html('Voltar');
    } else {
        $('.campoCadastro').css('display', '');
        $('.campoLeitura').css('display', 'none');

        $('#btnSalvar').css('display', '');
        $('#btnCancelar').removeClass('btn-primary');
        $('#btnCancelar').addClass('btn-danger');
        $('#btnCancelar > label').html('Cancelar');
    }
}

function inicializarRegistrosParaLoteRps(chaveRegistro) {
    mostrarPopup();

    var auxOption = $('<option value="">Informe o registro</option>');
    $('#LoteRps').append(auxOption);

    $.ajax({
        url: 'LoteRps/GetParaChaveEstrangeira',
        type: 'POST',
        success: function (result) {
            var dados = result.data;
            for (var i = 0; i < dados.length; i++) {
                auxOption = $('<option value="' + dados[i].chaveRegistro + '">' + dados[i].valorRegistro + '</option>');
                $('#LoteRps').append(auxOption);
            }
            $('#LoteRps').selectize();

            if (undefined == chaveRegistro) {
                $('#LoteRps')[0].selectize.setValue('');
            } else {
                $('#LoteRps')[0].selectize.setValue(chaveRegistro);
            }
            $('#formCadastro').data('bootstrapValidator').updateStatus('LoteRps', 'NOT_VALIDATED');
            
            registrosObtidosParaLoteRps = true;

            fecharPopup();
        },
        error: function (request, status, error) {
            $('#LoteRps').selectize();
            mostrarMsgErro('Falha ao tentar obter os registros, favor tente novamente');
            fecharPopup();
        }
    });
}

function inicializarRegistrosParaPrestador(chaveRegistro) {
    mostrarPopup();

    var auxOption = $('<option value="">Informe o registro</option>');
    $('#Prestador').append(auxOption);

    $.ajax({
        url: 'Prestador/GetParaChaveEstrangeira',
        type: 'POST',
        success: function (result) {
            var dados = result.data;
            for (var i = 0; i < dados.length; i++) {
                auxOption = $('<option value="' + dados[i].chaveRegistro + '">' + dados[i].valorRegistro + '</option>');
                $('#Prestador').append(auxOption);
            }
            $('#Prestador').selectize();

            if (undefined == chaveRegistro) {
                $('#Prestador')[0].selectize.setValue('');
            } else {
                $('#Prestador')[0].selectize.setValue(chaveRegistro);
            }
            
            registrosObtidosParaPrestador = true;

            fecharPopup();
        },
        error: function (request, status, error) {
            $('#Prestador').selectize();
            mostrarMsgErro('Falha ao tentar obter os registros, favor tente novamente');
            fecharPopup();
        }
    });
}

function inicializarRegistrosParaTomador(chaveRegistro) {
    mostrarPopup();

    var auxOption = $('<option value="">Informe o registro</option>');
    $('#Tomador').append(auxOption);

    $.ajax({
        url: 'Tomador/GetParaChaveEstrangeira',
        type: 'POST',
        success: function (result) {
            var dados = result.data;
            for (var i = 0; i < dados.length; i++) {
                auxOption = $('<option value="' + dados[i].chaveRegistro + '">' + dados[i].valorRegistro + '</option>');
                $('#Tomador').append(auxOption);
            }
            $('#Tomador').selectize();

            if (undefined == chaveRegistro) {
                $('#Tomador')[0].selectize.setValue('');
            } else {
                $('#Tomador')[0].selectize.setValue(chaveRegistro);
            }
            
            registrosObtidosParaTomador = true;

            fecharPopup();
        },
        error: function (request, status, error) {
            $('#Tomador').selectize();
            mostrarMsgErro('Falha ao tentar obter os registros, favor tente novamente');
            fecharPopup();
        }
    });
}

function limparFormCadastro() {
    $('#btnSalvarContinuar').css('display', '');
    $('#btnSalvar').removeData('idRegistro');

    ajustarBotoes(false);

    $('#Id').val('');
    $('#Numero').val('');
    $('#Serie').val('');
    $('#Tipo').val('');
    $('#DataEmissao').val('');
    $('#Status').val('');
    $('#Competencia').val('');
    $('#ValorServico').val('');
    $('#ISSRetido').val('');
    $('#ItemListaServico').val('');
    $('#CodigoCnae').val('');
    $('#CodigoTributacaoMunicipio').val('');
    $('#Discriminacao').val('');
    $('#CodigoMunicipio').val('');
    $('#ExigibilidadeISS').val('');
    $('#MunicipioIncidencia').val('');
    $('#OptanteSimplesNacional').val('');
    $('#IncentivoFiscal').val('');

    if (registrosObtidosParaLoteRps) {
        $('#LoteRps')[0].selectize.setValue('');
        $('#formCadastro').data('bootstrapValidator').updateStatus('LoteRps', 'NOT_VALIDATED');
    }    

    if (registrosObtidosParaPrestador) {
        $('#Prestador')[0].selectize.setValue('');
    }    

    if (registrosObtidosParaTomador) {
        $('#Tomador')[0].selectize.setValue('');
    }    

    var listaSelects =
    [
        {
            registrosObtidos: registrosObtidosParaLoteRps,
            idRegistroEscolhido: undefined,
            funcaoInicializacao: inicializarRegistrosParaLoteRps
        },
        {
            registrosObtidos: registrosObtidosParaPrestador,
            idRegistroEscolhido: undefined,
            funcaoInicializacao: inicializarRegistrosParaPrestador
        },
        {
            registrosObtidos: registrosObtidosParaTomador,
            idRegistroEscolhido: undefined,
            funcaoInicializacao: inicializarRegistrosParaTomador
        }
    ];

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'), listaSelects);
}

function carregarFormCadastro(IdLoteRps, Numero, Serie, Tipo, DataEmissao, Status, Competencia, ValorServico, ISSRetido, ItemListaServico, CodigoCnae, CodigoTributacaoMunicipio, Discriminacao, CodigoMunicipio, ExigibilidadeISS, MunicipioIncidencia, CpfCnpj_Prestador, CpfCnpj_Tomador, OptanteSimplesNacional, IncentivoFiscal, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').data('idRegistro', {
        Id: Id
    });

    ajustarBotoes(false);

    $('#Id').val(Id);
    $('#Numero').val(Numero);
    $('#Serie').val(Serie);
    $('#Tipo').val(Tipo);
    $('#DataEmissao').val(DataEmissao);
    $('#Status').val(Status);
    $('#Competencia').val(Competencia);
    $('#ValorServico').val(ValorServico);
    $('#ISSRetido').val(ISSRetido);
    $('#ItemListaServico').val(ItemListaServico);
    $('#CodigoCnae').val(CodigoCnae);
    $('#CodigoTributacaoMunicipio').val(CodigoTributacaoMunicipio);
    $('#Discriminacao').val(Discriminacao);
    $('#CodigoMunicipio').val(CodigoMunicipio);
    $('#ExigibilidadeISS').val(ExigibilidadeISS);
    $('#MunicipioIncidencia').val(MunicipioIncidencia);
    $('#OptanteSimplesNacional').val(OptanteSimplesNacional);
    $('#IncentivoFiscal').val(IncentivoFiscal);

    if (registrosObtidosParaLoteRps) {
        $('#LoteRps')[0].selectize.setValue(IdLoteRps);
        $('#formCadastro').data('bootstrapValidator').updateStatus('LoteRps', 'NOT_VALIDATED');
    }    

    if (registrosObtidosParaPrestador) {
        $('#Prestador')[0].selectize.setValue(CpfCnpj_Prestador);
    }    

    if (registrosObtidosParaTomador) {
        $('#Tomador')[0].selectize.setValue(CpfCnpj_Tomador);
    }    

    var listaSelects =
    [
        {
            registrosObtidos: registrosObtidosParaLoteRps,
            idRegistroEscolhido: IdLoteRps,
            funcaoInicializacao: inicializarRegistrosParaLoteRps
        },
        {
            registrosObtidos: registrosObtidosParaPrestador,
            idRegistroEscolhido: CpfCnpj_Prestador,
            funcaoInicializacao: inicializarRegistrosParaPrestador
        },
        {
            registrosObtidos: registrosObtidosParaTomador,
            idRegistroEscolhido: CpfCnpj_Tomador,
            funcaoInicializacao: inicializarRegistrosParaTomador
        }
    ];

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'), listaSelects);
}

metodoDetalhes = true;
function exibirDetalhes(IdLoteRps, Numero, Serie, Tipo, DataEmissao, Status, Competencia, ValorServico, ISSRetido, ItemListaServico, CodigoCnae, CodigoTributacaoMunicipio, Discriminacao, CodigoMunicipio, ExigibilidadeISS, MunicipioIncidencia, CpfCnpj_Prestador, CpfCnpj_Tomador, OptanteSimplesNacional, IncentivoFiscal, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').removeData('idRegistro');

    ajustarBotoes(true);

    $('#IdLeitura').html(Id);
    $('#LoteRpsLeitura').html(IdLoteRps);
    $('#NumeroLeitura').html(Numero);
    $('#SerieLeitura').html(Serie);
    $('#TipoLeitura').html(Tipo);
    $('#DataEmissaoLeitura').html(DataEmissao);
    $('#StatusLeitura').html(Status);
    $('#CompetenciaLeitura').html(Competencia);
    $('#ValorServicoLeitura').html(ValorServico);
    $('#ISSRetidoLeitura').html(ISSRetido);
    $('#ItemListaServicoLeitura').html(ItemListaServico);
    $('#CodigoCnaeLeitura').html(CodigoCnae);
    $('#CodigoTributacaoMunicipioLeitura').html(CodigoTributacaoMunicipio);
    $('#DiscriminacaoLeitura').html(Discriminacao);
    $('#CodigoMunicipioLeitura').html(CodigoMunicipio);
    $('#ExigibilidadeISSLeitura').html(ExigibilidadeISS);
    $('#MunicipioIncidenciaLeitura').html(MunicipioIncidencia);
    $('#PrestadorLeitura').html(CpfCnpj_Prestador);
    $('#TomadorLeitura').html(CpfCnpj_Tomador);
    $('#OptanteSimplesNacionalLeitura').html(OptanteSimplesNacional);
    $('#IncentivoFiscalLeitura').html(IncentivoFiscal);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

//metodoDelete = 'Rps/Delete';
function colunasTabelaRemocao(objetoTabela) {
    return '{ \'Id\': \'' + objetoTabela.Id + '\' }';
}

//metodoUpdate = 'Rps/Update';
function colunasTabelaAlteracao(objetoTabela) {
    var auxNumero = isNullOrEmpty(objetoTabela.Numero) ? '' : objetoTabela.Numero;
    var auxSerie = isNullOrEmpty(objetoTabela.Serie) ? '' : objetoTabela.Serie;
    var auxTipo = isNullOrEmpty(objetoTabela.Tipo) ? '' : objetoTabela.Tipo;
    var auxDataEmissao = isNullOrEmpty(objetoTabela.DataEmissao) ? '' : objetoTabela.DataEmissao;
    var auxStatus = isNullOrEmpty(objetoTabela.Status) ? '' : objetoTabela.Status;
    var auxCompetencia = isNullOrEmpty(objetoTabela.Competencia) ? '' : objetoTabela.Competencia;
    var auxValorServico = isNullOrEmpty(objetoTabela.ValorServico) ? '' : objetoTabela.ValorServico;
    var auxISSRetido = isNullOrEmpty(objetoTabela.ISSRetido) ? '' : objetoTabela.ISSRetido;
    var auxItemListaServico = isNullOrEmpty(objetoTabela.ItemListaServico) ? '' : objetoTabela.ItemListaServico;
    var auxCodigoCnae = isNullOrEmpty(objetoTabela.CodigoCnae) ? '' : objetoTabela.CodigoCnae;
    var auxCodigoTributacaoMunicipio = isNullOrEmpty(objetoTabela.CodigoTributacaoMunicipio) ? '' : objetoTabela.CodigoTributacaoMunicipio;
    var auxDiscriminacao = isNullOrEmpty(objetoTabela.Discriminacao) ? '' : objetoTabela.Discriminacao;
    var auxCodigoMunicipio = isNullOrEmpty(objetoTabela.CodigoMunicipio) ? '' : objetoTabela.CodigoMunicipio;
    var auxExigibilidadeISS = isNullOrEmpty(objetoTabela.ExigibilidadeISS) ? '' : objetoTabela.ExigibilidadeISS;
    var auxMunicipioIncidencia = isNullOrEmpty(objetoTabela.MunicipioIncidencia) ? '' : objetoTabela.MunicipioIncidencia;
    var auxCpfCnpj_Prestador = isNullOrEmpty(objetoTabela.CpfCnpj_Prestador) ? '' : objetoTabela.CpfCnpj_Prestador;
    var auxCpfCnpj_Tomador = isNullOrEmpty(objetoTabela.CpfCnpj_Tomador) ? '' : objetoTabela.CpfCnpj_Tomador;
    var auxOptanteSimplesNacional = isNullOrEmpty(objetoTabela.OptanteSimplesNacional) ? '' : objetoTabela.OptanteSimplesNacional;
    var auxIncentivoFiscal = isNullOrEmpty(objetoTabela.IncentivoFiscal) ? '' : objetoTabela.IncentivoFiscal;
    return '\'' + objetoTabela.IdLoteRps + '\', \'' + auxNumero + '\', \'' + auxSerie + '\', \'' + auxTipo + '\', \'' + auxDataEmissao + '\', \'' + auxStatus + '\', \'' + auxCompetencia + '\', \'' + auxValorServico + '\', \'' + auxISSRetido + '\', \'' + auxItemListaServico + '\', \'' + auxCodigoCnae + '\', \'' + auxCodigoTributacaoMunicipio + '\', \'' + auxDiscriminacao + '\', \'' + auxCodigoMunicipio + '\', \'' + auxExigibilidadeISS + '\', \'' + auxMunicipioIncidencia + '\', \'' + auxCpfCnpj_Prestador + '\', \'' + auxCpfCnpj_Tomador + '\', \'' + auxOptanteSimplesNacional + '\', \'' + auxIncentivoFiscal + '\', \'' + objetoTabela.Id + '\'';
}

function montarDadosCadastro() {
    return {
        Id: $('#Id').val(),
        Numero: $('#Numero').val(),
        Serie: $('#Serie').val(),
        Tipo: $('#Tipo').val(),
        DataEmissao: $('#DataEmissao').val(),
        Status: $('#Status').val(),
        Competencia: $('#Competencia').val(),
        ValorServico: $('#ValorServico').val(),
        ISSRetido: $('#ISSRetido').val(),
        ItemListaServico: $('#ItemListaServico').val(),
        CodigoCnae: $('#CodigoCnae').val(),
        CodigoTributacaoMunicipio: $('#CodigoTributacaoMunicipio').val(),
        Discriminacao: $('#Discriminacao').val(),
        CodigoMunicipio: $('#CodigoMunicipio').val(),
        ExigibilidadeISS: $('#ExigibilidadeISS').val(),
        MunicipioIncidencia: $('#MunicipioIncidencia').val(),
        OptanteSimplesNacional: $('#OptanteSimplesNacional').val(),
        IncentivoFiscal: $('#IncentivoFiscal').val(),
        IdLoteRps: $('#LoteRps').val(),
        CpfCnpj_Prestador: $('#Prestador').val(),
        CpfCnpj_Tomador: $('#Tomador').val()
    };
}

$(document).ready(function () {
    inicializarTabelaPrincipal($('#tabelaPrincipal'), colunasTabelaPrincipal);

    options.fields = camposFormCadastro;
    $('#formCadastro').bootstrapValidator(options);

    $('#btnNovo').click(function () {
        limparFormCadastro();
    });

    $('#btnSalvar').click(function () {
        var auxUrl;
        var auxData = montarDadosCadastro();

        var auxId = $(this).data('idRegistro');
        if (undefined == auxId) {
            auxUrl = metodoInsert;
        } else {
            auxUrl = metodoUpdate;
            auxData.Id = auxId.Id;
        }

        salvarRegistro($('#formCadastro'), $('#dvFormCadastro'), $('#dvTabelaPrincipal'), auxUrl, auxData);
    });

    $('#btnSalvarContinuar').click(function () {
        var auxUrl = metodoInsert;
        var auxData = montarDadosCadastro();
        salvarRegistro($('#formCadastro'), $('#dvFormCadastro'), $('#dvTabelaPrincipal'), auxUrl, auxData, undefined, true);
    });

    $('#btnCancelar').click(function () {
        mostrarDvTabelaPrincipal($('#formCadastro'), $('#dvFormCadastro'), $('#dvTabelaPrincipal'), undefined);
    });
});