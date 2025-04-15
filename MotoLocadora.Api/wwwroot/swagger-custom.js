(function () {
    window.addEventListener("load", function () {
        const bearerPrefix = "Bearer ";

        const originalFetch = window.fetch;
        window.fetch = function () {
            return originalFetch.apply(this, arguments).then(async response => {
                try {
                    const clonedResponse = response.clone();
                    const jsonResponse = await clonedResponse.json();

                    if (arguments[0].includes("/api/Auth/login")) {
                        const token = jsonResponse.token;
                        if (token && window.ui) {
                            const bearerToken = bearerPrefix + token;

                            // Usa a API do Swagger UI para autenticar
                            const authorization = {
                                Bearer: {
                                    name: "Authorization",
                                    schema: {
                                        type: "apiKey",
                                        in: "header",
                                        name: "Authorization",
                                        description: ""
                                    },
                                    value: bearerToken
                                }
                            };

                            window.ui.preauthorizeApiKey("Bearer", bearerToken);
                            console.log("JWT token aplicado ao Swagger:", bearerToken);
                        }
                    }
                } catch (error) {
                    console.error("Erro ao aplicar token automaticamente:", error);
                }

                return response;
            });
        };
    });
})();
