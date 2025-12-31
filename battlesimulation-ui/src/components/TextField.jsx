export default function TextField( {label, id, value, onChange} ) {

    return (
        <div className='text-field'>
            <label>{label}</label> <br/>
            <input type='text'
            id={id}
            value={value}
            onChange={e => onChange(e.target.value)}
            />
        </div>
    )
}