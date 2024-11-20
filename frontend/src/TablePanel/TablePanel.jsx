import React, { useEffect, useState } from "react";
import "./TablePanel.css";
import { getTable } from "../Services/FilesService";

const TablePanel = ({ fileId, fileName, onClose }) => {
    const [tableData, setTableData] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchTableData = async () => {
            const data = await getTable(fileId);
            setTableData(data);
            setLoading(false);
        };

        fetchTableData();
    }, [fileId]);

    if (loading) {
        return (
            <div className="table-panel">
                <p>Loading...</p>
            </div>
        );
    }

    return (
        <div className="table-panel">
            <div className="table-panel-header">
                <h2>{fileName}</h2>
                <button onClick={onClose}>Close</button>
            </div>
            <div className="table-panel-content">
                <table>
                    <thead>
                        <tr>
                            <th rowSpan="2">Б/сч</th>
                            <th colSpan="2">ВХОДЯЩЕЕ САЛЬДО</th>
                            <th colSpan="2">ОБОРОТЫ</th>
                            <th colSpan="2">ИСХОДЯЩЕЕ САЛЬДО</th>
                        </tr>
                        <tr>
                            <th>Актив</th>
                            <th>Пассив</th>
                            <th>Дебет</th>
                            <th>Кредит</th>
                            <th>Актив</th>
                            <th>Пассив</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableData.classes.map((cls) => (
                            <React.Fragment key={cls.classNumber}>
                                <tr>
                                    <td colSpan="7">
                                        {cls.classNumber} {cls.className}
                                    </td>
                                </tr>
                                {cls.groups.map((group) => (
                                    <React.Fragment key={group.groupNumber}>
                                        {group.records.map((record, idx) => (
                                            <tr key={idx}>
                                                <td>{record.accountNumber}</td>
                                                <td>{record.openingActive}</td>
                                                <td>{record.openingPassive}</td>
                                                <td>{record.turnoverDebit}</td>
                                                <td>{record.turnoverCredit}</td>
                                                <td>{record.closingActive}</td>
                                                <td>{record.closingPassive}</td>
                                            </tr>
                                        ))}
                                        <tr className="bold-row">
                                            <td>{group.groupNumber}</td>
                                            <td>{group.groupTotal.openingActive}</td>
                                            <td>{group.groupTotal.openingPassive}</td>
                                            <td>{group.groupTotal.turnoverDebit}</td>
                                            <td>{group.groupTotal.turnoverCredit}</td>
                                            <td>{group.groupTotal.closingActive}</td>
                                            <td>{group.groupTotal.closingPassive}</td>
                                        </tr>
                                    </React.Fragment>
                                ))}
                                <tr className="bold-row">
                                    <td>ПО КЛАССУ</td>
                                    <td>{cls.classTotal.openingActive}</td>
                                    <td>{cls.classTotal.openingPassive}</td>
                                    <td>{cls.classTotal.turnoverDebit}</td>
                                    <td>{cls.classTotal.turnoverCredit}</td>
                                    <td>{cls.classTotal.closingActive}</td>
                                    <td>{cls.classTotal.closingPassive}</td>
                                </tr>
                            </React.Fragment>
                        ))}
                        <tr className="bold-row">
                            <td>БАЛАНС</td>
                            <td>{tableData.globalTotal.openingActive}</td>
                            <td>{tableData.globalTotal.openingPassive}</td>
                            <td>{tableData.globalTotal.turnoverDebit}</td>
                            <td>{tableData.globalTotal.turnoverCredit}</td>
                            <td>{tableData.globalTotal.closingActive}</td>
                            <td>{tableData.globalTotal.closingPassive}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default TablePanel;
