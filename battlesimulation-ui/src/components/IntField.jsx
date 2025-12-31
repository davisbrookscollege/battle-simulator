export default function IntField( {label, id, value, onChange} ) {

    return (
        <div className='int-field'>
            <label>{label}</label> <br/>
            <input 
            type='number'
            id={id}
            value={value}
            onChange={e => onChange(Number(e.target.value))}
            />
        </div>
    )
}