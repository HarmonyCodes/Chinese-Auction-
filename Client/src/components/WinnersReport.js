import React, { useEffect, useState } from 'react';
import api from '../services/api';

const WinnersReport = () => {
  const [winners, setWinners] = useState([]);

  useEffect(() => {
    api.get('/raffle/winners')
      .then(res => setWinners(res.data))
      .catch(() => setWinners([]));
  }, []);

  return (
    <div>
      <h2>דוח זוכים</h2>
      <ul>
        {winners.map(w => (
          <li key={w.id}>{w.name} - {w.giftName}</li>
        ))}
      </ul>
    </div>
  );
};

export default WinnersReport;
