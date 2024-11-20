export const deleteFile = async (id) => {
    const response = await fetch(`/api/files/delete/${id}`, { method: "DELETE" });
    return response;
};

export const uploadFile = async (formData) => {
    const response = await fetch('/api/files/upload', {
        method: "POST",
        body: formData
    });

    return response.json();
};

export const getAllFiles = async () => {
    const response = await fetch("/api/files/getall", {
        method: "GET",
    });

    return response.json();
}

export const getTable = async (fileId) => {
    const response = await fetch(`/api/files/gettable/${fileId}`);
    return response.json();
};

export const downloadFile = async (fileId, fileName) => {
    const response = await fetch(`/api/files/download/${fileId}`, {
        method: 'GET',
    });

    if (!response.ok) {
        throw new Error('Failed to fetch the file');
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();

    link.remove();
    window.URL.revokeObjectURL(url);
};