import React, { useState } from 'react';
import api from '../services/api';

const Register = () => {
  const [form, setForm] = useState({
    userName: '',
    fullName: '',
    phone: '',
    email: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    setError('');
    setSuccess(false);
    try {
      await api.post('/auth/register', form);
      setSuccess(true);
    } catch (err) {
      setError(err.response?.data || 'שגיאה בהרשמה');
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <h2>הרשמה</h2>
      <input name="userName" placeholder="שם משתמש" value={form.userName} onChange={handleChange} />
      <br />
      <input name="fullName" placeholder="שם מלא" value={form.fullName} onChange={handleChange} />
      <br />
      <input name="phone" placeholder="טלפון" value={form.phone} onChange={handleChange} />
      <br />
      <input name="email" placeholder="אימייל" value={form.email} onChange={handleChange} />
      <br />
      <input name="password" type="password" placeholder="סיסמה" value={form.password} onChange={handleChange} />
      <br />
      <button type="submit">הרשם</button>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      {success && <div style={{ color: 'green' }}>נרשמת בהצלחה!</div>}
    </form>
  );
};

export default Register;
