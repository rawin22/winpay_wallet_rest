# WinPay Wallet

WinPay Wallet is a robust financial wallet application built with **ASP.NET Core 9.0**. It provides a few examples of features for managing user accounts, financial transactions, foreign exchange deals, and instant payments, integrated with secure authentication and multi-language support.

## 🚀 Features

* **Financial Operations**
  * **Balance Management**: Real-time balance checking and updates.
  * **Account Statements**: Detailed transaction history and statement generation.
  * **Instant Payments**: Fast and secure payment processing.
  * **Foreign Exchange (FxDeal)**: Manage currency exchange and deals.
  * **Liquidation Preferences**: Configure user liquidation settings.

* **Security & Authentication**
  * **Secure Authentication**: Cookie-based authentication with custom middleware for automatic token refreshing.
  * **Data Protection**: Key storage persistence for secure data handling.
  * **User Verification**: Integration with verification services (KYC/GetVerified).

* **Utilities & Tools**
  * **QR Code Support**: Generate and read QR codes for transactions or identification.
  * **Multi-language Support**: Dynamic language loading and translation services.
  * **File Attachments**: Secure handling of file uploads and attachments.
  * **URL Shortener**: Internal service for managing short URLs.
  * **Email Notifications**: Integrated email service for user alerts and communications.

## 🛠️ Technology Stack

* **Framework**: [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) (ASP.NET Core)
* **Frontend**: Razor Pages, Blazor Server (Interactive Server Components)
* **Data Access**: Dapper, Microsoft.Data.SqlClient
* **Database**: SQL Server (Implied)
* **Key Libraries**:
  * `Blazored.TextEditor` & `TinyMCE`: Rich text editing.
  * `QRCoder` & `ZXing.Net`: QR code generation and scanning.
  * `SkiaSharp`: Cross-platform 2D graphics API.
  * `Swashbuckle`: Swagger tooling for API documentation.

## 📂 Project Structure

* **`Wallet/`**: Main application project.
  * **`Program.cs`**: Application entry point and service configuration.
  * **`GPWebApi/`**: Integration with Global Payments Web API (DTOs and Data Objects).
  * **`Helper/`**: Business logic helpers (e.g., `AccountStatementHelper`, `FxDealHelper`).
  * **`Interfaces/`**: Service contracts and interface definitions.
  * **`Services/`**: Core application services (e.g., `EmailService`, `LanguageService`).
  * **`Models/`**: Data models and ViewModels.
  * **`Pages/`**: Razor Pages for the web UI.
  * **`KeyStore/`**: Directory for persisting data protection keys.

## 📋 Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* SQL Server (or compatible database)
* Visual Studio 2022 or VS Code

## ⚙️ Configuration

The application relies on `appsettings.json` for configuration. Ensure you have the necessary settings configured, including:

* **Connection Strings**: Database connection details.
* **Win:Beta:NotaryNodes**: Configuration for notary nodes.
* **Authentication Settings**: Token endpoints and secrets.

## 🚀 Getting Started

1. **Clone the repository**

    ```bash
    git clone https://github.com/rawin22/winpay_wallet.git
    cd winpay_wallet
    ```

2. **Restore dependencies**

    ```bash
    dotnet restore
    ```

3. **Run the application**

    ```bash
    cd Wallet
    dotnet run
    ```

4. **Access the application**
    Open your browser and navigate to `https://localhost:7166` (or the port indicated in the console).

## 🔐 Authentication Flow

The application uses a custom middleware pipeline to handle authentication:

1. Checks for an existing `AuthToken` cookie.
2. Automatically attempts to refresh the access token via `ITsgCoreServiceHelper` if needed.
3. Redirects unauthenticated users to `/auth/login`.

## 📄 License

[Add License Information Here]
