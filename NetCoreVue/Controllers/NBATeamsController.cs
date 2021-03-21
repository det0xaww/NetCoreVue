using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using NetCoreVue.Helpers;
using NetCoreVue.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NetCoreVue.ViewModels;

namespace NetCoreVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NBATeamsController : ControllerBase
    {
        #region PropertiesAndConstructors
        private static List<Team> Teams;
        private static string _searchTerm;

        public NBATeamsController()
        {
            if (Teams == null)
                Teams = GetAllNBATeams();
        }
        #endregion

        #region Index

        private List<Team> GetAllNBATeams()
        {
            try
            {
                var client = new RestClient("https://free-nba.p.rapidapi.com/teams");
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-host", "free-nba.p.rapidapi.com");
                request.AddHeader("x-rapidapi-key", "057aad95c0mshb1f8a56da3b3400p1e53d6jsn6b8416c66ab2");
                request.AddHeader("useQueryString", "true");
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute(request);
                //RestSharp.Serialization.Json.JsonDeserializer deserial = new RestSharp.Serialization.Json.JsonDeserializer();
                //var x = deserial.Deserialize<List<Team>>(response);
                GetContent<Team> content = JsonConvert.DeserializeObject<GetContent<Team>>(response.Content);

                return content.data;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("getteams/{searchTerm?}")]
        public ActionResult<List<Team>> GetTeams(string searchTerm = null)
        {
            List<Team> teams = new List<Team>();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                _searchTerm = searchTerm.ToLower().Trim();
                teams = Teams.Where(x => x.abbreviation.ToLower().Trim().Contains(_searchTerm) ||
                                         x.city.ToLower().Trim().Contains(_searchTerm) ||
                                         x.conference.ToLower().Trim().Contains(_searchTerm) ||
                                         x.division.ToLower().Trim().Contains(_searchTerm) ||
                                         x.full_name.ToLower().Trim().Contains(_searchTerm) ||
                                         x.name.ToLower().Trim().Contains(_searchTerm)).ToList();
            }
            else
                teams = Teams;

            return teams;
        }
        #endregion

        #region Add
        [Authorize]
        [HttpPost("add")]
        public IActionResult Add([FromBody] TeamViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                model.id = Guid.NewGuid().ToString();
                Team team = model;
                Teams.Add(team);
                    return new JsonResult(new Notifications("foo", "success", "Add", "The team is successfully added"));
            }
            catch(Exception ex)
            {
                return new JsonResult(new Notifications("foo", "danger", "Add Error", "Something went wrong, an error occured"));
            }
        }
        #endregion

        #region Edit
        [Authorize]
        [HttpGet("edit/{id}")]
        public ActionResult<TeamViewModel> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            TeamViewModel team = Teams.SingleOrDefault(x => x.id == id);

            return team;
        }

        [Authorize]
        [HttpPut("edit/{id}")]
        public IActionResult Edit(string id, [FromBody] TeamViewModel model)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                Team team = Teams.SingleOrDefault(x => x.id == id);
                int index = Teams.IndexOf(team);
                model.id = id;
                Team teamToEdit = model;
                Teams[index] = teamToEdit;
                return new JsonResult(new Notifications("foo", "success", "Edit", "The team is successfully updated"));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Notifications("foo", "danger", "Edit Error", "Something went wrong, an error occured"));
            }
        }
        #endregion

        #region Delete
        [Authorize]
        [HttpPost("delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            Team team = Teams.SingleOrDefault(x => x.id == id);

            if (team == null)
                return NotFound();
            else
            {
                try
                {
                    Teams.Remove(team);

                    return new JsonResult(new Notifications("foo", "success", "Delete", "The team is successfully deleted"));
                }
                catch (Exception ex)
                {
                    return new JsonResult(new Notifications("foo", "danger", "Delete Error", "Something went wrong, an error occured"));
                }
            }
        }
        #endregion

        #region GeneratePdfFile
        [HttpGet("getfile")]
        public async Task<IActionResult> Download()
        {
            CreatePdf();

            Stream stream = System.IO.File.OpenRead(@"files\NBATeams.pdf");

            if (stream == null)
                return NotFound();

            return File(stream, "application/pdf");
        }


        private void CreatePdf()
        {
            //Create document  
            Document doc = new Document();
            //Create PDF Table  
            PdfPTable tableLayout = new PdfPTable(6);
            //Create a PDF file in specific path  
            var fileStream = new FileStream(@"files\NBATeams.pdf", FileMode.Create, FileAccess.ReadWrite);
            PdfWriter.GetInstance(doc, fileStream);
            //Open the PDF document  
            doc.Open();
            //Add Content to PDF  
            doc.Add(Add_Content_To_PDF(tableLayout));
            // Closing the document  
            doc.Close();
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = {
                30,
                20,
                30,
                30,
                30,
                30
            }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
                                              //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("NBA Teams", new Font(Font.FontFamily.HELVETICA, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 6,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            //Add header  
            AddCellToHeader(tableLayout, "Abbreviation");
            AddCellToHeader(tableLayout, "City");
            AddCellToHeader(tableLayout, "Conference");
            AddCellToHeader(tableLayout, "Division");
            AddCellToHeader(tableLayout, "Fullname");
            AddCellToHeader(tableLayout, "Name");

            //Add body
            var list = Teams;
            if (!string.IsNullOrEmpty(_searchTerm))
            {
                list = Teams.Where(x => x.abbreviation.ToLower().Trim().Contains(_searchTerm) ||
                                         x.city.ToLower().Trim().Contains(_searchTerm) ||
                                         x.conference.ToLower().Trim().Contains(_searchTerm) ||
                                         x.division.ToLower().Trim().Contains(_searchTerm) ||
                                         x.full_name.ToLower().Trim().Contains(_searchTerm) ||
                                         x.name.ToLower().Trim().Contains(_searchTerm)).ToList();
            }
            else
                list = Teams;
            foreach (var item in list)
            {
                AddCellToBody(tableLayout, item.abbreviation);
                AddCellToBody(tableLayout, item.city);
                AddCellToBody(tableLayout, item.conference);
                AddCellToBody(tableLayout, item.division);
                AddCellToBody(tableLayout, item.full_name);
                AddCellToBody(tableLayout, item.name);
            }

            return tableLayout;
        }
        // Method to add single cell to the header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }
        #endregion
    }
}
