// SecureAndObserve.IoT.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <iomanip>
#include <chrono>
#include <ctime>
#include <sstream>
#include <tuple>
#include <curl/curl.h>
#include <nlohmann/json.hpp>

using json = nlohmann::json;

struct ServiceInfo {
    std::string typeOfService;
    std::string securityLevel;
};
struct Order
{
    std::string id;
    std::string typeOfService;
    std::string securityLevel;
};
struct GuardReportInfo
{
    std::string Date;
    std::string Message;
    std::string Description;
};


static size_t WriteCallback(void* contents, size_t size, size_t nmemb, std::string* s) {
    size_t newLength = size * nmemb;
    size_t oldLength = s->size();
    try {
        s->resize(oldLength + newLength);
    }
    catch (std::bad_alloc& e) {
        // Handle memory problem
        return 0;
    }

    std::copy((char*)contents, (char*)contents + newLength, s->begin() + oldLength);
    return newLength;
}


void performGetRequest(const std::string& url, json& getData) {
    CURL* curl;
    CURLcode res;
    std::string readBuffer;

    curl = curl_easy_init();
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

        res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "GET request failed: " << curl_easy_strerror(res) << std::endl;
        }
        else {
            try {
                getData = json::parse(readBuffer);
                std::cout << "Get data success!\n" << std::endl;
            }
            catch (json::parse_error& e) {
                std::cerr << "JSON parse error: " << e.what() << std::endl;
            }
        }

        curl_easy_cleanup(curl);
    }
}


void performPostRequest(const std::string& url, const json& jsonData) {
    CURL* curl;
    CURLcode res;
    std::string readBuffer;

    curl = curl_easy_init();
    if (curl) {
        std::string jsonString = jsonData.dump();

        struct curl_slist* headers = nullptr;
        headers = curl_slist_append(headers, "Content-Type: application/json");

        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
        curl_easy_setopt(curl, CURLOPT_POST, 1L);
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, jsonString.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

        res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "POST request failed: " << curl_easy_strerror(res) << std::endl;
        }
        else {
            try {
                auto jsonResponse = json::parse(readBuffer);
                std::cout << "POST Response JSON:\n" << jsonResponse.dump(4) << std::endl;
            }
            catch (json::parse_error& e) {
                std::cerr << "JSON parse error: " << e.what() << std::endl;
            }
        }

        curl_slist_free_all(headers);
        curl_easy_cleanup(curl);
    }
}

void performGetRequestOrderIds(const std::string& url, std::vector<std::string>& orderIds) {
    CURL* curl;
    CURLcode res;
    std::string readBuffer;

    curl = curl_easy_init();
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

        res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "GET request failed: " << curl_easy_strerror(res) << std::endl;
        }
        else {
            try {
                auto jsonResponse = json::parse(readBuffer);
                if (jsonResponse.is_array()) {
                    for (const auto& item : jsonResponse) {
                        if (item.contains("orderId")) {
                            orderIds.push_back(item["orderId"]);
                        }
                    }
                }
                else {
                    std::cerr << "Expected JSON array." << std::endl;
                }
            }
            catch (json::parse_error& e) {
                std::cerr << "JSON parse error: " << e.what() << std::endl;
            }
        }

        curl_easy_cleanup(curl);
    }
}

ServiceInfo performGetRequest(const std::string& url) {
    CURL* curl;
    CURLcode res;
    std::string readBuffer;

    curl = curl_easy_init();
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

        res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "GET request failed: " << curl_easy_strerror(res) << std::endl;
        }
        else {
            try {
                auto jsonResponse = json::parse(readBuffer);
                for (const auto& item : jsonResponse) {
                    ServiceInfo info;
                    if (item.contains("typeOfService")) {

                        info.typeOfService = item["typeOfService"];
                    }
                    if (item.contains("securityLevel"))
                    {
                        info.securityLevel = item["securityLevel"];
                    }
                    return info;
                }
            }
            catch (json::parse_error& e) {
                std::cerr << "JSON parse error: " << e.what() << std::endl;
            }
        }

        curl_easy_cleanup(curl);
    }
    return {};
}

std::vector<GuardReportInfo> getGuardReports(const std::string& url) {
    std::vector<GuardReportInfo> reports;

    CURL* curl;
    CURLcode res;
    std::string readBuffer;

    curl = curl_easy_init();
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

        res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "GET request failed: " << curl_easy_strerror(res) << std::endl;
        }
        else {
            try {
                auto jsonResponse = json::parse(readBuffer);

                    for (const auto& report : jsonResponse) {
                        GuardReportInfo info;
                        if (report.contains("date") ) {
                           
                            info.Date = report["date"];
                        }
                        if (report.contains("message")) {
                            info.Message = report["message"];
                        }
                        if (report.contains("descriptions")) {
                            info.Description = report["descriptions"];
                            
                        }
                        reports.push_back(info);
                    }
            }
            catch (json::parse_error& e) {
                std::cerr << "JSON parse error: " << e.what() << std::endl;
            }
        }

        curl_easy_cleanup(curl);
    }

    return reports;
}

std::string getCurrentDateAsJsonString() {
    auto now = std::chrono::system_clock::now();
    std::time_t now_c = std::chrono::system_clock::to_time_t(now);
    std::tm timeInfo;
    localtime_s(&timeInfo, &now_c);

    std::ostringstream oss;
    oss << std::put_time(&timeInfo, "%FT%T"); 
    return oss.str();
}

using namespace std;

int main() {
    // Open file appSettings.json
    std::ifstream inputFile("appSettings.json");
    if (!inputFile.is_open()) {
        std::cerr << "Can't open file appSettings.json" << std::endl;
        return 1;
    }
    json settings;
    inputFile >> settings;
    std::string serverUrl = settings["serverUrl"];
    std::cout << "URL: " << serverUrl << std::endl;
    //

    const string guardExstentionsId = "e320b881-34f0-4f21-8672-00011f936404";

    string getUrl = serverUrl + "/api/GuardExstensionsApi/" + guardExstentionsId;
    string postUrl = serverUrl + "/api/GuardReportsApi/";

    std::cout << "Performing GET request...\n";

    json getData;
    string userId;
    performGetRequest(getUrl, getData);
    if (getData.contains("userId")) {
        userId = getData["userId"];
        std::cout << "User found!" << std::endl;
    }
    else {
        std::cerr << "User not found" << std::endl;
    }
    //
    getUrl = serverUrl + "/api/UsersApi/" + userId;
    string userNickname;
    performGetRequest(getUrl, getData);
    if (getData.contains("userNickname")) {
        userNickname = getData["userNickname"];
        std::cout << "User nickname found!" << std::endl;
    }
    else {
        std::cerr << "User nickname not found" << std::endl;
    }
    cout << endl;
    cout << endl;
    cout << endl;
    cout << "Welcome, " + userNickname << endl;
    //
    getUrl = serverUrl + "/api/OrderGuardsApi/" + guardExstentionsId;
    std::vector<std::string> orderIds;
    performGetRequestOrderIds(getUrl, orderIds);
    //
    getUrl = serverUrl + "/api/OrdersApi/";
    std::vector<Order> orders;
    for (const auto& orderId : orderIds) {
        std::string url = getUrl + orderId;
        ServiceInfo service = performGetRequest(url);
        Order order;
        order.id = orderId;
        order.typeOfService = service.typeOfService;
        order.securityLevel = service.securityLevel;
        orders.push_back(order);
        std::cout << " " << orders.capacity() << ". TypeOfService: " << service.typeOfService << ", SecurityLevel: " << service.securityLevel << std::endl;
    }
    string choiceOrderNumber = "-1";
    cout << "Choose number of order to view options: " << endl;
    cin >> choiceOrderNumber;
    string choiceOptions = "-1";
    while (choiceOptions != "4")
    {
        cout << "Choose options: " << endl;
        cout << " 1. See all reports " << endl;
        cout << " 2. Write report " << endl;
        cout << " 3. Show statistics " << endl;
        cout << " 4. Exit" << endl;
        cin >> choiceOptions;
        if (choiceOptions == "1")
        {
            string orderId = orders[stoi(choiceOrderNumber) - 1].id;
            getUrl = serverUrl + "/api/GuardReportsApi/" + orderId;
            std::vector<GuardReportInfo> reports = getGuardReports(getUrl);
            for (const auto& report : reports) {
                std::cout << "Date: " << report.Date << ", Message: " << report.Message << ", Description: " << report.Description << std::endl;
            }
        }
        if (choiceOptions == "2")
        {
            string orderId = orders[stoi(choiceOrderNumber) - 1].id;
            string message;
            cout << "Write your message (Stable, Suspicious, Dangerous)" << endl;
            cin >> message;
            string descriptions;
            cout << "Write your descriptions" << endl;
            cin >> descriptions;
            string date = getCurrentDateAsJsonString();
            json postData = {
        {"GuardExstensionsId", guardExstentionsId},
        {"OrderId",orderId},
        {"Date", date},
        {"Message", message},
        {"Descriptions", descriptions}
            };
            performPostRequest(postUrl, postData);
        }
        if (choiceOptions == "3")
        {
            string orderId = orders[stoi(choiceOrderNumber) - 1].id;
            getUrl = serverUrl + "/api/GuardReportsApi/" + orderId;
            std::vector<GuardReportInfo> reports = getGuardReports(getUrl);
            double stableCount = 0;
            double suspiciousCount = 0;
            double dangerousCount = 0;
            for (const auto& report : reports) {
                if (report.Message == "Stable")
                    stableCount++;
                if (report.Message == "Suspicious")
                    suspiciousCount++;
                if (report.Message == "Dangerous")
                    dangerousCount++;
            }
            cout << "Stable reports: " << stableCount / reports.capacity() * 100 << endl;
            cout << "Suspicious reports: " << suspiciousCount / reports.capacity() * 100 << endl;
            cout << "Dangerous reports: " << dangerousCount / reports.capacity() * 100 << endl;
        }
    }
    //

    return 0;
}


// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
