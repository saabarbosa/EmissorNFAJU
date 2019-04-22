var camposFormLogin = {
    Username: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    },
    Password: {
        validators: {
            notEmpty: {
                message: 'Campo obrigat&oacute;rio'
            }
        }
    }
};

function montarDados() {
    return {
        Username: $('#Username').val(),
        Password: $('#Password').val()
    };
}

metodoLogin = 'Login/Validar';

$(document).ready(function () {

    options.fields = camposFormLogin;
    $('#formLogin').bootstrapValidator(options);

    $('#btnEntrar').click(function () {
        var auxUrl = metodoLogin;
        var auxData = montarDados();

        Logar($('#formLogin'), $('#dvFormLogin'), auxUrl, auxData);
    });

});


function Logar(auxFormLogin, auxDvFormLogin, auxUrl, auxData) {
    auxFormLogin.data('bootstrapValidator').validate();
    if (auxFormLogin.data('bootstrapValidator').isValid()) {
        mostrarPopup(); //carregando popup
        $.ajax({
            url: auxUrl,
            type: 'POST',
            data: auxData,
            success: function (result) {

                fecharPopup();
                var msgErro = result;

                if (undefined != msgErro && '' != msgErro.msgErro.toString().trim()) {
                    mostrarMsgErro(result.msgErro);
                } else {
                    location.href = 'DadosEnvio';
                }
               
            },
            error: function (request, status, error) {
                fecharPopup();
                mostrarMsgErro('Falha na conex&atilde;o, favor tente novamente');
            }
        });
    }
}