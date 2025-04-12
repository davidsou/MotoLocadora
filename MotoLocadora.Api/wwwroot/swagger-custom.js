(function () {
    window.addEventListener("load", function () {
        const bearerPrefix = "Bearer ";

        const originalFetch = window.fetch;
        window.fetch = function () {
            return originalFetch.apply(this, arguments).then(async response => {
                try {
                    const clonedResponse = response.clone();
                    const jsonResponse = await clonedResponse.json();

                    // Verifica se a chamada foi para o endpoint de login
                    if (arguments[0].includes('/api/Auth/login')) {
                        const token = jsonResponse.token;
                        if (token) {
                            console.log('Token JWT interceptado do login:', token);
                            const bearerToken = bearerPrefix + token;

                            // Encontra o campo de token do Swagger UI e preenche automaticamente
                            const tokenInput = document.querySelector('input[placeholder="Bearer token"]');
                            if (tokenInput) {
                                tokenInput.value = bearerToken;
                                // Simula clique no botão "Authorize"
                                const authorizeButton = document.querySelector('.auth-wrapper .btn.modal-btn.auth.authorize');
                                if (authorizeButton) {
                                    authorizeButton.click();
                                }
                            }
                        }
                    }
                } catch (error) {
                    console.error('Swagger Auto Auth Error:', error);
                }

                return response;
            });
        };
    });
})();
