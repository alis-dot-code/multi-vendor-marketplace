import toast from 'react-hot-toast';

export const successToast = (msg: string) => toast.success(msg);
export const errorToast = (msg: string) => toast.error(msg);
export const infoToast = (msg: string) => toast(msg);

export default toast;
