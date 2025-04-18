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
                            window.ui.preauthorizeApiKey("Bearer", bearerToken);
                            showToast("Token JWT aplicado com sucesso! ✅");
                        }
                    }
                } catch (error) {
                    console.error("Erro ao aplicar token automaticamente no Swagger:", error);
                }

                return response;
            });
        };

        createDevTokenButton();
    });

    function showToast(message) {
        const toast = document.createElement("div");
        toast.textContent = message;
        toast.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background-color: #28a745;
            color: white;
            padding: 10px 20px;
            border-radius: 5px;
            font-family: sans-serif;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
            z-index: 9999;
        `;
        document.body.appendChild(toast);
        setTimeout(() => toast.remove(), 3000);
    }

    function createDevTokenButton() {
        const button = document.createElement("button");
        button.textContent = "🔐 Preencher token de exemplo";
        button.style.cssText = `
            position: fixed;
            bottom: 20px;
            right: 20px;
            background-color: #007bff;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            cursor: pointer;
            z-index: 9999;
        `;

        button.onclick = () => {
            const devToken = "Bearer SEU_TOKEN_AQUI";
            if (window.ui) {
                window.ui.preauthorizeApiKey("Bearer", devToken);
                showToast("Token de exemplo aplicado! 🔑");
            }
        };

        document.body.appendChild(button);
    }
})();
