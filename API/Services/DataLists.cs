

namespace NeotechAPI.Services;

public class DataLists
{
    public List<string> Generations { get; set; }
    public List<string> Origins { get; set; }
    public List<string> Spheres { get; set; }
    public List<string> Contracts { get; set; }
    public List<string> Experiences { get; set; }
    public List<string> UrgesAndTrips { get; set; }
    public List<string> Networks { get; set; }
    public List<string> Expertises { get; set; }
    public List<string> Genders { get; set; }
    public List<string> Names { get; set; }
    public List<string> BirthLottery { get; set; }
    public List<string> Genes { get; set; }
    public List<string> TraumaCard { get; set; }
    public DateTime LastUpdated { get; set; }


    private IConfiguration _config;
    private HttpClient _client { get; set; }
    public DataLists(IConfiguration config)
    {
        _client = new HttpClient();
        _config = config;
    }

    private Task<HttpResponseMessage> GetGoogleSheet(string sheetName)
    {
        var googleUrl = "https://sheets.googleapis.com/v4/spreadsheets";
        var googleApiKey = _config["GoogleApiKey"];
        var googleSpreadsheetId = _config["GoogleSpreadsheetId"];

        var sheetUrl = $"{googleUrl}/{googleSpreadsheetId}/values/{sheetName}?alt=json&key={googleApiKey}";
        if (sheetName == "Names") sheetUrl += "&majorDimension=COLUMNS";

        return _client.GetAsync(sheetUrl);
    }

    private async Task<GoogleResponse?> Deserialize(Task<HttpResponseMessage> response)
    {
        return await JsonSerializer.DeserializeAsync<GoogleResponse>(await response.Result.Content.ReadAsStreamAsync());
    }


    public class GoogleResponse
    {
        [JsonPropertyName("values")]
        public List<object> Values { get; set; }
    }

    public class GoogleResponseGeneration
    {
        [JsonPropertyName("values")]
        public List<object> Values { get; set; }
    }

    public async Task<HttpResponseMessage> Update(string sheetName)
    {
        var generationsTask = await GetGoogleSheet("Generations");
        return generationsTask;
    }
}
        // .ContinueWith(async antecedent => {
        //     var generations = Deserialize(antecedent).Result?.Values.Select(value => value as GoogleResponseGeneration);
        // }





            // const formattedData = this.genericList(allRows);
        //     formattedData.forEach(row =>
        //     {
        //         row["attributes"] = Array.from(row["attributes"].slice(1, row["attributes"].length - 1).split(", ").map(attribute => parseInt(attribute)));
        //     });
        //     return formattedData;
        // });


//     ProcessGenericList(allRows)
//     {
//             var formattedData = new List<string>();
//             const headers = allRows.shift();
//             const renamedHeaders = [];
//             for (let header of headers)
//             {
//                 renamedHeaders.push(this.amendSheetOrColumnName(header));
//             }
//             allRows.forEach(row =>
//             {
//                 const rowData = { };
//                 row.forEach((item, index) =>
//                 {
//                     rowData[renamedHeaders[index]] = item;
//                 });
//                 formattedData.push(rowData);
//             });
//             return formattedData;
//         }


//         Generations =
//         Origins =
//         Spheres =
//         Contracts =
//         Experiences =
//         UrgesAndTrips =
//         Networks =
//         Expertises =
//         Genders =
//         Names =
//         BirthLottery =
//         Genes =
//         TraumaCard =


//     let nameList = this.nameList.bind(this);
//         let genericList = this.genericList.bind(this);
//         let contractList = this.contractList.bind(this);
//         let experienceList = this.experienceList.bind(this);
//         let generationList = this.generationList.bind(this);
//         let originSphereList = this.originSphereList.bind(this);

//         promises.push(this.getGoogleSheet("Generations", generationList));
//         promises.push(this.getGoogleSheet("Spheres", originSphereList));
//         promises.push(this.getGoogleSheet("Origins", originSphereList));
//         promises.push(this.getGoogleSheet("Genders", genericList));
//         promises.push(this.getGoogleSheet("Names", nameList));
//         promises.push(this.getGoogleSheet("Contracts", contractList));
//         promises.push(this.getGoogleSheet("Networks", genericList));
//         promises.push(this.getGoogleSheet("Experiences", genericList));
//         promises.push(this.getGoogleSheet("Urges & Trips", genericList));
//         return Promise.all(promises);

//     }


//     //     fetch(url).then(response => response.json()).then(jsonData => {
//     //         try {
//     //             this[this.amendSheetOrColumnName(sheetName)] = processor(jsonData.values);
//     //             resolve("Success");
//     //         } catch(error) {
//     //             console.log(`Failed to process ${sheetName} due to: ${error}`);
//     //             resolve("Failure");
//     //         }
//     //     })
//     //     .catch(error => console.log(`Failed to get data from server due to: ${error}`));
//     // });


//     // Amend the name of a sheet or column to work as a variable name  
//     private string AmendSheetOrColumnName(string name)
//     {
//         name = name.Replace(" & ", "And");
//         name = name.Replace("'s ", "");
//         name = name.Replace(" ", "");
//         name = name.Replace(" ", "");
//         if (name == "UUID") name.ToLower();
//         name = name.ElementAt(0).ToString().ToLower() + name.Substring(1);
//         return name;
//     }

//     ProcessGenerationList(allRows)
//     {

//     }




//     // Data Processors



//     nameList(allColumns)
//     {
//         const formattedData = { };
//         allColumns.forEach((columnData) =>
//         {
//             const header = this.amendSheetOrColumnName(columnData.shift());
//             formattedData[header] = columnData;
//         });
//         return formattedData;
//     }
//     contractList(allRows)
//     {
//         const formattedData = this.genericList(allRows);
//         formattedData.forEach(row =>
//         {
//             const originalText = row["edge"];
//             const cutText = originalText.slice(3);
//             const options = cutText.split(" or ");
//             row["edge"] = { "option1": options[0], "option2": options[1]};
//         });
//         return formattedData;
//     }
//     experienceList(allRows)
//     {

//     }
//     originSphereList(allRows)
//     {
//         const formattedData = this.genericList(allRows);
//         formattedData.forEach(row =>
//         {
//             row["experienceTableExists"] = (row["experienceTableExists"] == "Yes") ? true : false
//             });
//         return formattedData;
//     }
// }