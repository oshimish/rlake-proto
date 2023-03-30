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
import { AuthenticationType, data } from 'azure-maps-control';
import { useParams } from 'react-router';
import { AppContext } from '../AppContext';
import { Point } from '../api';


const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
}

const markersLayer: IAzureMapLayerType = 'SymbolLayer';


const Map: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { state } = useContext(AppContext);
    const { mapRef, isMapReady } = useContext<IAzureMapsContextProps>(AzureMapsContext);

    const [mapOptions, setMapOptions] = useState<IAzureMapOptions>(option);

    const setMapCenterAndZoom = useCallback(
        (point: data.Position, zoom: number) => {
            if (mapRef) {
                // Simple Camera options modification
                mapRef.setCamera({ center: point, zoom });
            }
        },
        [mapRef],
    )

    useEffect(() => {
        const point = state.points.find(p => p.id === id);
        if (point) {
            setMapCenterAndZoom(new data.Position(point.longitude!, point.latitude!), 7);
        };
    }, [id, state.points, setMapCenterAndZoom])

    return (
        <AzureMap options={mapOptions}>
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
                            image: 'pin-round-blue', // You can use other icons from Azure Maps
                        },
                        textOptions: {
                            textField: ['get', 'reason'], // Specify the property name that contains the text you want to appear with the symbol
                            offset: [0, 3.8],
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