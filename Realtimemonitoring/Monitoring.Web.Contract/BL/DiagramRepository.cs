using System;
using System.Collections.Generic;
using System.Linq;
using Sychev.Monitoring.Web.Contract.Models.Outcoming.Shared;

namespace Sychev.Monitoring.Web.Contract.BL
{
    public sealed class DiagramRepository : IDiagramRepository
    {
        private static readonly List<DiagramModel> Diagrams = new List<DiagramModel>
            {
                new DiagramModel
                {
                    Name = "Number of active workflow instances",
                    Id=Guid.Parse("0AF02C07-A9B5-4845-B856-629B2F983F43"), 
                    Lines = new List<DiagramLineModel>
                    {
                        new DiagramLineModel
                        {
                            Name="Instances",
                            Id = Guid.Parse("AB0B4D11-525F-4FC7-AB0C-90EE3BA800AF")
                        }
                    },
                    DiagramType = DiagramType.Seismograph
                },
                new DiagramModel
                {
                    Name = "Number of active workflow instances by types",
                    Id=Guid.Parse("28319487-F922-49F7-8EB2-03F276568149"),
                    Lines = new List<DiagramLineModel>
                    {
                        new DiagramLineModel
                        {
                            Name="Instances Allocate",
                            Id = Guid.Parse("A48A208A-6DB5-48D7-8519-A15F1941FCE2")
                        },
                        new DiagramLineModel
                        {
                            Name="Instances Fetch",
                            Id = Guid.Parse("8B1262FF-3453-4259-B2AD-22D4A9B3DDBE")
                        },
                        new DiagramLineModel
                        {
                            Name="Instances Update",
                            Id = Guid.Parse("B3274645-CDBD-4B8B-B6AC-CF5B352DE047")
                        }
                    },
                    DiagramType = DiagramType.Seismograph
                },
                new DiagramModel
                {
                    Name = "Number of errors",
                    Id=Guid.Parse("7BC52D54-E6E2-45D2-94EA-9BF3AA8B2E36"), 
                    Lines = new List<DiagramLineModel>
                    {
                        new DiagramLineModel
                        {
                            Name="Count",
                            Id = Guid.Parse("5441AB06-BE31-4FB8-8303-2D5456744741")
                        }
                    },
                    DiagramType = DiagramType.Seismograph
                },
                new DiagramModel
                {
                    Name = "Temperature during the day.",
                    Id=Guid.Parse("E25BC622-365C-42CF-B24D-98573E6C0673"), 
                    Lines = new List<DiagramLineModel>
                    {
                        new DiagramLineModel
                        {
                            Name="Temperature",
                            Id = Guid.Parse("3818ABC3-7611-4FE1-9231-BD3CA76CA28E")
                        }
                    },
                    DiagramType = DiagramType.Seismograph
                },
            };


        public static List<DiagramModel> GetDiagrams()
        {
            return Diagrams;
        }
        public static DiagramModel GetDiagram(Guid id)
        {
            return Diagrams.First(i => i.Id == id);
        }


        public List<DiagramModel> GetDiagramsList()
        {
            return Diagrams;
        }

        public DiagramModel GetDiagramById(Guid id)
        {
            return Diagrams.First(i => i.Id == id);
        }
    }
}