import React, { useCallback, useContext, useEffect, useRef, useState } from 'react';
import {
    AzureMap,
    IAzureMapOptions,
    AzureMapDataSourceProvider,
    AzureMapFeature,
    IAzureMapsContextProps,
    AzureMapsContext,
    IAzureMapLayerType,
    AzureMapLayerProvider,

} from 'react-azure-maps';
import * as azureMap from "react-azure-maps";
import { AuthenticationType, data, ControlOptions } from 'azure-maps-control';
import { useParams } from 'react-router';
import { AppContext } from '../AppContext';
import { Point } from '../api';


const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
    // https://learn.microsoft.com/en-us/azure/azure-maps/supported-map-styles
    style: "satellite_road_labels",
    view: 'Auto',
}

const markersLayer: IAzureMapLayerType = 'SymbolLayer';

const cameraOptions: azureMap.AzureSetCameraOptions = {
    center: [-1.9591947375679695, 52.46891727965905],
    maxBounds: [-6.0, 49.959999905, 1.68153079591, 58.6350001085],
};

const controls: azureMap.IAzureMapControls[] = [
    {
        controlName: "StyleControl",
        controlOptions: {
            mapStyles: ['road', 'grayscale_dark', 'night', 'road_shaded_relief', 'satellite', 'satellite_road_labels']
        },
        options: {
            position: "top-right",
        } as ControlOptions,
    },
    {
        controlName: "ZoomControl",
        options: { position: "top-right" } as ControlOptions,
    },
    {
        controlName: "PitchControl",
        options: { position: "top-right" } as ControlOptions,
    },
    {
        controlName: "CompassControl",
        options: { position: "top-right" } as ControlOptions,
    },
];

const Map: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { state } = useContext(AppContext);
    const { mapRef, isMapReady } = useContext<IAzureMapsContextProps>(AzureMapsContext);

    const [mapOptions, setMapOptions] = useState<IAzureMapOptions>(option);

    const setMapCenterAndZoom = useCallback(
        (point: data.Position, zoom: number) => {
            if (mapRef) {
                // Simple Camera options modification
                mapRef.setCamera({
                    center: point,
                    zoom,
                    duration: 2000,
                    type: 'fly'
                });
            }
        },
        [mapRef],
    )

    const setPoint = useCallback(
        (point?: Point) => {
            if (point) {
                setMapCenterAndZoom(new data.Position(point.longitude!, point.latitude!), 7);
            };
        },
        [setMapCenterAndZoom],
    )

    useEffect(() => {
        const point = state.points.find(p => p.id === id);
        setPoint(point);
    }, [id, state.points, setPoint])

    useEffect(() => {
        if (!mapRef) return;
        //mapRef && mapRef.setView('Auto');
    }, [mapRef])

    return (
        <AzureMap options={mapOptions}
            controls={controls}
            cameraOptions={cameraOptions}>

            <AzureMapDataSourceProvider id="pointsDataSource"
                events={{
                    dataadded: (e: any) => {
                        console.log('Data on source added', e);
                    },
                }}>
                <AzureMapLayerProvider
                    id="markersLayer"
                    options={{
                        iconOptions: {
                            image: 'pin-red', // You can use other icons from Azure Maps
                            size: 1.5,
                        },
                        textOptions: {
                            textField: ['get', 'reason'], // Specify the property name that contains the text you want to appear with the symbol
                            offset: [0, 3.8],
                            haloBlur: 1,
                            haloColor: "#000000",
                            haloWidth: 2,
                            size: 26,
                            color: "#ff0000",
                        },
                    }}
                    type={markersLayer}
                />
                {state.points.map((point) => (
                    <AzureMapFeature
                        key={point.id}
                        id={point.id}
                        type="Point"
                        coordinate={new data.Position(point.longitude!, point.latitude!)}
                        properties={{ ...point }}
                    />
                ))}
            </AzureMapDataSourceProvider>
        </AzureMap>
    );
};


export default Map;