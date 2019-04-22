var camposFormCadastro = {
    Id: {
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
        data: 'Regra',
        name: 'Regra'
    },
    {
        data: 'CpfCnpj_Prestador',
        name: 'CpfCnpj_Prestador'
    },
    {
        data: 'Discriminacao',
        name: 'Discriminacao'
    },
    {
        data: 'Expressoes',
        name: 'Expressoes'
    },
    {
        render: renderColunaOpcoes
    }
];
metodoGet = 'Regra/Get';
metodoInsert = 'Regra/Insert';

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
    $('#Regra').val('');
    $('#CpfCnpj_Prestador').val('');
    $('#Discriminacao').val('');
    $('#Expressoes').val('');

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

function carregarFormCadastro(Regra, CpfCnpj_Prestador, Discriminacao, Expressoes, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').data('idRegistro', {
        Id: Id
    });

    ajustarBotoes(false);

    $('#Id').val(Id);
    $('#Regra').val(Regra);
    $('#CpfCnpj_Prestador').val(CpfCnpj_Prestador);
    $('#Discriminacao').val(Discriminacao);
    $('#Expressoes').val(Expressoes);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

metodoDetalhes = true;
function exibirDetalhes(Regra, CpfCnpj_Prestador, Discriminacao, Expressoes, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').removeData('idRegistro');

    ajustarBotoes(true);

    $('#IdLeitura').html(Id);
    $('#RegraLeitura').html(Regra);
    $('#CpfCnpj_PrestadorLeitura').html(CpfCnpj_Prestador);
    $('#DiscriminacaoLeitura').html(Discriminacao);
    $('#ExpressoesLeitura').html(Expressoes);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

metodoDelete = 'Regra/Delete';
function colunasTabelaRemocao(objetoTabela) {
    return '{ \'Id\': \'' + objetoTabela.Id + '\' }';
}

metodoUpdate = 'Regra/Update';
function colunasTabelaAlteracao(objetoTabela) {
    var auxRegra = isNullOrEmpty(objetoTabela.Regra) ? '' : objetoTabela.Regra;
    var auxCpfCnpj_Prestador = isNullOrEmpty(objetoTabela.CpfCnpj_Prestador) ? '' : objetoTabela.CpfCnpj_Prestador;
    var auxDiscriminacao = isNullOrEmpty(objetoTabela.Discriminacao) ? '' : objetoTabela.Discriminacao;
    var auxExpressoes = isNullOrEmpty(objetoTabela.Expressoes) ? '' : objetoTabela.Expressoes;
    return '\'' + auxRegra + '\', \'' + auxCpfCnpj_Prestador + '\', \'' + auxDiscriminacao + '\', \'' + auxExpressoes + '\', \'' + objetoTabela.Id + '\'';
}

function montarDadosCadastro() {
    return {
        Id: $('#Id').val(),
        Regra: $('#Regra').val(),
        CpfCnpj_Prestador: $('#CpfCnpj_Prestador').val(),
        Discriminacao: $('#Discriminacao').val(),
        Expressoes: $('#Expressoes').val()
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