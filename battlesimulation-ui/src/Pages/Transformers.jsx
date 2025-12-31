import { useEffect, useState } from 'react'
import { fetchByFaction, runBattle } from '../services/api';  
import CheckList from '../components/CheckList';
import ModularDisplay from '../components/ModularDisplay';

function Transformers() {
    
    const [autobots, setAutobots] = useState([]);
    const [decepticons, setDecepticons] = useState([]);
    const [currentView, setCurrentView] = useState(null); 
    const [currentData, setCurrentData] = useState(null);
    const [selected, setSelected] = useState([]);

    async function loadFactions() {
        try {
            const autobotsData = await fetchByFaction('Autobots');
            const decepticonsData = await fetchByFaction('Decepticons');

            setAutobots(autobotsData);
            setDecepticons(decepticonsData);
        } catch (err) {
            console.error('API error:', err);
        }
    }

    useEffect(() => {
        loadFactions();
    }, []);

    
    function toggle(id) {
        setSelected(prev => {
            const updated = prev.includes(id)
            ? prev.filter(x => x !== id)
            : [...prev, id]

            //Sorts the selected IDs for consistency
            return updated.sort((a, b) => a - b);
        });
      }
    
    return (
        <div className='transformers-battle-simulator-page'>
            <h2>Autobots:</h2>

            <CheckList
                items={autobots}
                selected={selected}
                onToggle={toggle}
            />

            <h2>Decepticons:</h2>

            <CheckList
                items={decepticons}
                selected={selected}
                onToggle={toggle}
            />

            <button onClick={async () => {
              const results = await runBattle(selected);
              setCurrentView('battleResults');
              setCurrentData(results);
            }}
            >
              Battle
            </button>
            
            <button
                onClick={async () => {
                    setCurrentView('fighterInfo');
                    setCurrentData(selected);
                }}
            >
              Info
            </button>

            <button
              onClick={async () => {
                setCurrentView('createFighter');
                setCurrentData({
                    onDone: () => {
                        loadFactions();
                    }
                });
              }}
            >
              Create
            </button>

            <button
                onClick={async () => {
                setCurrentView('deleteFighter');
                setCurrentData({
                    ids: selected,
                    onDone: () => {
                        loadFactions();

                        //Clears selected so that deleted Transformers are not still selected
                        setSelected([]);
                    }
                });
              }}
            >
              Delete
            </button>

            <ModularDisplay view={currentView} data={currentData} />
        </div>
    );
}

export default Transformers;