export default function RadioButton({item, group, selected, onChange}) {

    return (
        <label className='radio-button'>
            <input
                type='radio'
                name={group}
                value={item.id}
                checked={selected === item.id}
                onChange={() => onChange(item.id)}
            />
            {item.name}
        </label>
    )
}