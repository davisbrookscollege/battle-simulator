import TransformerBattleResults from './TransformerBattleResults';
import TransformersInfo from './TransformersInfo';
import TransformersDelete from './TransformersDelete';
import TransformersCreate from './TransformersCreate';

export default function ModularDisplay({ view, data }){

    switch(view){
        case 'battleResults':
            return <TransformerBattleResults results={data} />;
        case 'fighterInfo':
            return <TransformersInfo ids={data} />;
        case 'createFighter':
            return <TransformersCreate data={data} />;
        case 'deleteFighter':
            return <TransformersDelete data={data} />;
        default:
            return (
            <div className='modular-display-default'>
                <h3>Transformers Battle Simulator</h3>
                <p>Created by Davis Brooks</p>
            </div>
        );
    }
}