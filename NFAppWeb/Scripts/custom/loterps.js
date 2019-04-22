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
        data: 'NumeroLote',
        name: 'NumeroLote'
    },
    {
        data: 'CpfCnpj',
        name: 'CpfCnpj'
    },
    {
        data: 'InscricaoMunicipal',
        name: 'InscricaoMunicipal'
    },
    {
        data: 'Quantidade',
        name: 'Quantidade'
    },
    {
        render: renderColunaOpcoes
    }
];
metodoGet = 'LoteRps/Get';
//metodoInsert = 'LoteRps/Insert';

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
    $('#NumeroLote').val('');
    $('#CpfCnpj').val('');
    $('#InscricaoMunicipal').val('');
    $('#Quantidade').val('');

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

function carregarFormCadastro(NumeroLote, CpfCnpj, InscricaoMunicipal, Quantidade, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').data('idRegistro', {
        Id: Id
    });

    ajustarBotoes(false);

    $('#Id').val(Id);
    $('#NumeroLote').val(NumeroLote);
    $('#CpfCnpj').val(CpfCnpj);
    $('#InscricaoMunicipal').val(InscricaoMunicipal);
    $('#Quantidade').val(Quantidade);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

metodoDetalhes = true;
function exibirDetalhes(NumeroLote, CpfCnpj, InscricaoMunicipal, Quantidade, Id) {
    $('#btnSalvarContinuar').css('display', 'none');
    $('#btnSalvar').removeData('idRegistro');

    ajustarBotoes(true);

    $('#IdLeitura').html(Id);
    $('#NumeroLoteLeitura').html(NumeroLote);
    $('#CpfCnpjLeitura').html(CpfCnpj);
    $('#InscricaoMunicipalLeitura').html(InscricaoMunicipal);
    $('#QuantidadeLeitura').html(Quantidade);

    mostrarDvFormCadastro($('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

//metodoDelete = 'LoteRps/Delete';
function colunasTabelaRemocao(objetoTabela) {
    return '{ \'Id\': \'' + objetoTabela.Id + '\' }';
}

//metodoUpdate = 'LoteRps/Update';
function colunasTabelaAlteracao(objetoTabela) {
    var auxNumeroLote = isNullOrEmpty(objetoTabela.NumeroLote) ? '' : objetoTabela.NumeroLote;
    var auxCpfCnpj = isNullOrEmpty(objetoTabela.CpfCnpj) ? '' : objetoTabela.CpfCnpj;
    var auxInscricaoMunicipal = isNullOrEmpty(objetoTabela.InscricaoMunicipal) ? '' : objetoTabela.InscricaoMunicipal;
    var auxQuantidade = isNullOrEmpty(objetoTabela.Quantidade) ? '' : objetoTabela.Quantidade;
    return '\'' + auxNumeroLote + '\', \'' + auxCpfCnpj + '\', \'' + auxInscricaoMunicipal + '\', \'' + auxQuantidade + '\', \'' + objetoTabela.Id + '\'';
}

function montarDadosCadastro() {
    return {
        Id: $('#Id').val(),
        NumeroLote: $('#NumeroLote').val(),
        CpfCnpj: $('#CpfCnpj').val(),
        InscricaoMunicipal: $('#InscricaoMunicipal').val(),
        Quantidade: $('#Quantidade').val()
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