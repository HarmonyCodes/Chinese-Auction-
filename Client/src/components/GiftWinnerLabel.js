import React from 'react';

const GiftWinnerLabel = ({ winner }) => {
  if (!winner) return null;
  return (
    <div style={{ marginTop: 8, color: '#388e3c', fontWeight: 600 }}>
      זכה: {winner.user?.userName || winner.userName || winner.UserName}
    </div>
  );
};

export default GiftWinnerLabel;
