import { useEffect, useState } from 'react';
import { fetchTransformer } from '../services/api';
import pluralize from "../Utilities/Pluralize";
import RenderFighterList from '../Utilities/RenderFighterList';

export default function TransformerBattleResults({results}) {

    const [winners, setWinners] = useState([]);
    const [losers, setLosers] = useState([]);
    const numWinners = results.winners.length;
    const numLosers = results.losers.length;
    const noFighters = (numWinners === 0 && numLosers === 0);

    useEffect(() => {
        async function fetchFighterResults() {
            const winnerData = await Promise.all(
                results.winners.map(winnerId => fetchTransformer(winnerId))
            );

            const loserData = await Promise.all(
                results.losers.map(loserId => fetchTransformer(loserId))
            );

            setWinners(winnerData);
            setLosers(loserData);
        }
        fetchFighterResults();
    }, [results]);

    if (noFighters) {
        return (
            <div className='battle-results'>
                <h2>Battle Results:</h2>
                <h3>Unfortunately, the transformers were too chicken to show up.</h3>
                <p>So nothing interesting happened.</p>
            </div>
        );
    }

    else if (results.isTie) {
        return (
            <div className='battle-results'>
                <h2>Battle Results:</h2>
                <h3>It's a tie! So everyone lost.</h3>
                <h3>{pluralize(numLosers, 'Loser')}: </h3>
                <RenderFighterList fighters={losers} />
            </div>
        );
    }

    else {
        return (
            <div className='battle-results'>
                <h2>Battle Results:</h2>
                <h3>{pluralize(numWinners, 'Winner')}: </h3>
                <RenderFighterList fighters={winners} />

                <h3>{pluralize(numLosers, 'Loser')}: </h3>

                {numLosers > 0 ? (
                    <RenderFighterList fighters={losers} />
                ) : (
                    <p>There were no losers — it was a one‑sided beatdown.</p>
                )}
            </div>
        )
    }
}