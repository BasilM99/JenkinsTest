import axios from 'axios';
import { getToken } from '../../Helpers';

export const defaultParams = () => ({
  headers: { Authorization: `Bearer ${getToken()}` },
});

export default axios;
