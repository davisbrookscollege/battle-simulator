import { useEffect, useState } from 'react';
import { getFactionNames } from '../services/api';

export default function IndividualTransformerInfo({ transformer }) {

    const winToLossRate = transformer.wins / (transformer.losses || 1);
    const winToLossRateFormatted = winToLossRate.toFixed(3);
    const [factionNames, setFactionNames] = useState({});

    useEffect(() => {
        getFactionNames().then(({ factionMap }) => setFactionNames(factionMap));
    }, []);

    return (
        <div className='individual-transformer-info'>
            <h3>{transformer.name}:</h3>
            <ul>
                <li>Wins: {transformer.wins}</li>
                <li>Losses: {transformer.losses}</li>
                <li>Win to loss rate: {transformer.wins}:{transformer.losses} ({winToLossRateFormatted})</li>
                <li>Faction: {factionNames[transformer.faction]}</li>
                <li>Alternate Vehicle: {transformer.altVehicle}</li>
                <li>Strength: {transformer.strength}</li>
                <li>Intelligence: {transformer.intelligence}</li>
            </ul>
        </div>
    )
}