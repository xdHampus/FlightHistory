using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using FlightHistoryCore.Model;
using FlightHistoryCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FlightHistoryVisualizer
{
    class MapViewModel : INotifyPropertyChanged
    {

        public MapViewModel()
        {
            SetupMap();
            CreateGraphics();

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SetupMap()
        {

            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISChartedTerritory);

        }

        private void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            var planeTrailsOverlay = new GraphicsOverlay();

            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                planeTrailsOverlay
            };
            this.GraphicsOverlays = overlays;

            using (var db = new FlightDbContext())
            {

                
                foreach (var item in db.Flights.Include("Status").Include("Trails").Where(f => f.ScanCompleted.Value))
                {
                    //&& !item.Status.Live.Value && item.Status.Text.Contains("Landed")
                    if (item.Status != null )
                    {
                        // SampleShitDraw(item, planeTrailsOverlay);
                        planeTrailsOverlay.Graphics.Add(GraphicFromFlight(item.Trails));
                    }
                }
            }


        }

        private void SampleShitDraw(Flight item, GraphicsOverlay planeTrailsOverlay)
        {
            var color = GetRandomColor();

            foreach (var trail in item.Trails)
            {
                var mPoint = MapPointFromTrail(trail);

                // Create a symbol to define how the point is displayed.
                var pointSymbol = new SimpleMarkerSymbol
                {
                    Style = SimpleMarkerSymbolStyle.Circle,
                    Color = color,
                    Size = 10.0
                };

                // Add an outline to the symbol.
                pointSymbol.Outline = new SimpleLineSymbol
                {
                    Style = SimpleLineSymbolStyle.Solid,
                    Color = Color.Transparent,
                    Width = 2.0
                };
                var pointGraphic = new Graphic(mPoint, pointSymbol);
                planeTrailsOverlay.Graphics.Add(pointGraphic);

            }
        }


        private Random random = new Random();

        private System.Drawing.Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            // The error is here
        }

        private Graphic GraphicFromFlight(List<Trail> trails)
        {

            // Create a list of points that define a polyline.


            List<MapPoint> linePoints = new List<MapPoint>();
            linePoints
                .AddRange(trails
                    .ConvertAll(t => MapPointFromTrail(t)));

            var polyLine = new Polyline(linePoints);
            var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, GetRandomColor(), 3.0);
            return new Graphic(polyLine, polylineSymbol);
        }


        private MapPoint MapPointFromTrail(Trail trail)
        {
            return new MapPoint((double)trail.Lng, (double)trail.Lat, SpatialReferences.Wgs84);
        }



        private Map _map;
        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged();
            }
        }
        private GraphicsOverlayCollection _graphicsOverlays;
        public GraphicsOverlayCollection GraphicsOverlays
        {
            get { return _graphicsOverlays; }
            set
            {
                _graphicsOverlays = value;
                OnPropertyChanged();
            }
        }
    }
}
