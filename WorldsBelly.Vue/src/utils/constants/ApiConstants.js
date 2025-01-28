let baseUrl = "https://api-worldsbelly.azurewebsites.net";

if (
  window.location.host.includes("localhost")
) {
  baseUrl = "https://localhost:5001";
}

export const domain = baseUrl;
