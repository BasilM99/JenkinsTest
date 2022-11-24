import {
  shape, number, string, bool,
} from 'prop-types';

const todoCard = shape({
  data: {
    id: number.isRequired,
    title: string.isRequired,
    description: string.isRequired,
    priority: string.isRequired,
    completed: bool.isRequired,
    archived: bool.isRequired,
  },
  isFetching: bool.isRequired,
  error: shape(),
});

export default todoCard;
