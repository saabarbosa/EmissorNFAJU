var camposFormCadastro = {
    Id: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    Data: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    ArquivoImportacao: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    ConteudoArquivoImportacao: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    ArquivoRemessa: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    XMLRemessa: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    ArquivoRetorno: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    XMLRetorno: {
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
        data: 'Data',
        name: 'Data'
    },
    {
        data: 'ArquivoImportacao',
        name: 'ArquivoImportacao'
    },
    {
        data: 'ArquivoRemessa',
        name: 'ArquivoRemessa'
    },
    {
        data: 'ArquivoRetorno',
        name: 'ArquivoRetorno'
    },
    {
        render: renderColunaOpcoes
    }
];
metodoGet = 'DadosEnvio/Get';
metodoInsert = 'DadosEnvio/Insert';

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

function limparFormCadastro() {
    $('#btnSalvarContinuar').css('display', '');
    $('#btnSalvar').removeData('idRegistro');


    ajustarBotoes(false);

    $('#Id').val('');
    $('#Data').val('');
    $('#ArquivoImportacao').val('');
    $('#ConteudoArquivoImportacao').val('');
    $('#ArquivoRemessa').val('');
    $('#XMLRemessa').val('');
    $('#ArquivoRetorno').val('');
    $('#XMLRetorno').val('');

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

function carregarFormCadastro(Id, Data, ArquivoImportacao, ConteudoArquivoImportacao, ArquivoRemessa, XMLRemessa, ArquivoRetorno, XMLRetorno) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').data('idRegistro', {
        Id: Id
    });

    ajustarBotoes(false);

    $('#Id').val(Id);
    $('#Data').val(Data);
    $('#ArquivoImportacao').val(ArquivoImportacao);
    $('#ConteudoArquivoImportacao').val(ConteudoArquivoImportacao);
    $('#ArquivoRemessa').val(ArquivoRemessa);
    $('#XMLRemessa').val(XMLRemessa);
    $('#ArquivoRetorno').val(ArquivoRetorno);
    $('#XMLRetorno').val(XMLRetorno);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

metodoDetalhes = true;
function exibirDetalhes(Id, Data, ArquivoImportacao, ArquivoRemessa, ArquivoRetorno) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').removeData('idRegistro');

    ajustarBotoes(true);

    $('#IdLeitura').html(Id);
    $('#DataLeitura').html(Data);
    $('#ArquivoImportacaoLeitura').html(ArquivoImportacao);
    //$('#ConteudoArquivoImportacaoLeitura').html(ConteudoArquivoImportacao);
    $('#ArquivoRemessaLeitura').html(ArquivoRemessa);
    //$('#XMLRemessaLeitura').html(XMLRemessa);
    $('#ArquivoRetornoLeitura').html(ArquivoRetorno);
    //$('#XMLRetornoLeitura').html(XMLRetorno);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

//metodoDelete = 'DadosEnvio/Delete';
function colunasTabelaRemocao(objetoTabela) {
    return '{ \'Id\': \'' + objetoTabela.Id + '\' }';
}

//metodoUpdate = 'DadosEnvio/Update';
function colunasTabelaAlteracao(objetoTabela) {
    return '\'' + objetoTabela.Id + '\', \'' + objetoTabela.Data + '\', \'' + objetoTabela.ArquivoImportacao + '\', \'' + objetoTabela.ConteudoArquivoImportacao + '\', \'' + objetoTabela.ArquivoRemessa + '\', \'' + objetoTabela.XMLRemessa + '\', \'' + objetoTabela.ArquivoRetorno + '\', \'' + objetoTabela.XMLRetorno + '\'';
    //aqui eu passo os parametros...posso removera quebra de linha nesse ponto
    //return '\'' + objetoTabela.Id + '\', \'' + 'ops' + '\', \'' + objetoTabela.ArquivoImportacao + '\', \'' + objetoTabela.ConteudoArquivoImportacao + '\', \'' + objetoTabela.ArquivoRemessa + '\', \'' + objetoTabela.XMLRemessa + '\', \'' + objetoTabela.ArquivoRetorno + '\', \'' + objetoTabela.XMLRetorno + '\'';
}

//$('#btnNovo').css('display', 'none');
function montarDadosCadastro() {
   
    return {
        Id: $('#Id').val(),
        Data: $('#Data').val(),
        ArquivoImportacao: $('#ArquivoImportacao').val(),
        ConteudoArquivoImportacao: $('#ConteudoArquivoImportacao').val(),
        ArquivoRemessa: $('#ArquivoRemessa').val(),
        XMLRemessa: $('#XMLRemessa').val(),
        ArquivoRetorno: $('#ArquivoRetorno').val(),
        XMLRetorno: $('#XMLRetorno').val()
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