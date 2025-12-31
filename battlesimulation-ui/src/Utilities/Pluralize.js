export default function pluralize(count, singular, plural = null) {
    if (count === 1) {
        return singular;
    }
    else if (plural){
        return plural;
    }
    else {
        return singular + 's';
    }
}